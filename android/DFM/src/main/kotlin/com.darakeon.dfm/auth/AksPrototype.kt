package com.darakeon.dfm.auth

import android.annotation.TargetApi
import android.content.Context
import android.os.Build
import android.security.KeyPairGeneratorSpec
import android.security.keystore.KeyGenParameterSpec
import android.security.keystore.KeyProperties
import android.util.Base64
import java.math.BigInteger
import java.security.Key
import java.security.KeyPair
import java.security.KeyPairGenerator
import java.security.KeyStore
import java.security.PrivateKey
import java.util.Calendar
import javax.crypto.Cipher
import javax.security.auth.x500.X500Principal

class AksPrototype(private val context: Context) {
	private val keyStore: KeyStore = createAndroidKeyStore()
	private val hasMarshmallow = Build.VERSION.SDK_INT >= Build.VERSION_CODES.M

	private fun createAndroidKeyStore(): KeyStore {
		val keyStore = KeyStore.getInstance("AndroidKeyStore")
		keyStore.load(null)
		return keyStore
	}

	fun getKeys(): KeyPair {
		var masterKey = getAndroidKeyStoreAsymmetricKeyPair("MASTER_KEY")

		if (masterKey == null)
			masterKey = createAndroidKeyStoreAsymmetricKey("MASTER_KEY")

		return masterKey
	}

	private fun createAndroidKeyStoreAsymmetricKey(alias: String): KeyPair {
		val generator = KeyPairGenerator.getInstance("RSA", "AndroidKeyStore")

		if (hasMarshmallow) {
			initGeneratorWithKeyGenParameterSpec(generator, alias)
		} else {
			initGeneratorWithKeyPairGeneratorSpec(generator, alias)
		}

		// Generates Key with given spec and saves it to the KeyStore
		return generator.generateKeyPair()
	}

	@Suppress("DEPRECATION")
	private fun initGeneratorWithKeyPairGeneratorSpec(generator: KeyPairGenerator, alias: String) {
		val startDate = Calendar.getInstance()
		val endDate = Calendar.getInstance()
		endDate.add(Calendar.YEAR, 20)

		val builder = KeyPairGeneratorSpec.Builder(context)
			.setAlias(alias)
			.setSerialNumber(BigInteger.ONE)
			.setSubject(X500Principal("CN=${alias} CA Certificate"))
			.setStartDate(startDate.time)
			.setEndDate(endDate.time)

		generator.initialize(builder.build())
	}

	@TargetApi(Build.VERSION_CODES.M)
	private fun initGeneratorWithKeyGenParameterSpec(generator: KeyPairGenerator, alias: String) {
		val builder = KeyGenParameterSpec.Builder(
			alias, KeyProperties.PURPOSE_ENCRYPT or KeyProperties.PURPOSE_DECRYPT
		)
			.setBlockModes(KeyProperties.BLOCK_MODE_ECB)
			.setEncryptionPaddings(KeyProperties.ENCRYPTION_PADDING_RSA_PKCS1)

		generator.initialize(builder.build())
	}

	private fun getAndroidKeyStoreAsymmetricKeyPair(alias: String): KeyPair? {
		val privateKey = keyStore.getKey(alias, null) as PrivateKey?

		if (keyStore.getKey(alias, null) != keyStore.getKey(alias, null)) {
			throw Exception("Oh, goddess!")
		}

		val publicKey = keyStore.getCertificate(alias)?.publicKey
		return if (privateKey != null && publicKey != null) {
			KeyPair(publicKey, privateKey)
		} else {
			null
		}
	}

	fun removeAndroidKeyStoreKey(alias: String) = keyStore.deleteEntry(alias)

	val TRANSFORMATION_ASYMMETRIC = "RSA/ECB/PKCS1Padding"

	val cipher: Cipher = Cipher.getInstance(TRANSFORMATION_ASYMMETRIC)

	fun encrypt(data: String, key: Key?): String {
		cipher.init(Cipher.ENCRYPT_MODE, key)
		val bytes = cipher.doFinal(data.toByteArray())
		return Base64.encodeToString(bytes, Base64.DEFAULT)
	}

	fun decrypt(data: String, key: Key?): String {
		cipher.init(Cipher.DECRYPT_MODE, key)
		val encryptedData = Base64.decode(data, Base64.DEFAULT)
		val decodedData = cipher.doFinal(encryptedData)
		return String(decodedData)
	}

	companion object {
		fun test(context: Context) {
			val aks = AksPrototype(context)
			val keys1 = aks.getKeys()
			val keys2 = aks.getKeys()

			val message = "Hey, listen!"

			if (keys1.private != keys2.private)
				throw Exception("Wow, keys?!")

			if (keys1.public != keys2.public)
				throw Exception("Ahaaaaa!!!")

			val cipher = aks.encrypt(message, keys1.public)
			val result = aks.decrypt(cipher, keys2.private)

			if (result != message)
				throw Exception("Crypto messed up!")
		}
	}
}
