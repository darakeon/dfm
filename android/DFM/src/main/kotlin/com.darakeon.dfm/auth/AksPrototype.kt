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
	private val keyStoreType = "AndroidKeyStore"
	private val keyStoreAlias = "MASTER_KEY"
	private val algorithm = "RSA"
	private val transformationAsymmetric = "RSA/ECB/PKCS1Padding"

	private val hasMarshmallow = Build.VERSION.SDK_INT >= Build.VERSION_CODES.M

	private val keyStore = createKeyStore()
	private val cipher = createCipher()

	private fun createKeyStore(): KeyStore {
		val keyStore = KeyStore.getInstance(keyStoreType)
		keyStore.load(null)
		return keyStore
	}

	private fun createCipher() =
		Cipher.getInstance(transformationAsymmetric)

	fun getKeys() = getKeyPair() ?: createKeyPair()

	private fun getKeyPair(): KeyPair? {
		val privateKey = getPrivateKey() ?: return null
		val publicKey = getPublicKey() ?: return null

		return KeyPair(publicKey, privateKey)
	}

	private fun getPublicKey() =
		keyStore.getCertificate(keyStoreAlias)?.publicKey

	private fun getPrivateKey() =
		keyStore.getKey(keyStoreAlias, null) as PrivateKey?

	private fun createKeyPair(): KeyPair {
		val generator = createGenerator()

		if (hasMarshmallow) {
			initGenerator(generator)
		} else {
			initOldGenerator(generator)
		}

		return generator.generateKeyPair()
	}

	private fun createGenerator() =
		KeyPairGenerator.getInstance(algorithm, keyStoreType)

	@TargetApi(Build.VERSION_CODES.M)
	private fun initGenerator(generator: KeyPairGenerator) {
		val purpose =
			KeyProperties.PURPOSE_ENCRYPT or
			KeyProperties.PURPOSE_DECRYPT

		val builder = KeyGenParameterSpec.Builder(keyStoreAlias, purpose)
			.setBlockModes(KeyProperties.BLOCK_MODE_ECB)
			.setEncryptionPaddings(KeyProperties.ENCRYPTION_PADDING_RSA_PKCS1)

		generator.initialize(builder.build())
	}

	@Suppress("DEPRECATION")
	private fun initOldGenerator(generator: KeyPairGenerator) {
		val startDate = Calendar.getInstance()
		val endDate = Calendar.getInstance()
		endDate.add(Calendar.YEAR, 20)

		val builder = KeyPairGeneratorSpec.Builder(context)
			.setAlias(keyStoreAlias)
			.setSerialNumber(BigInteger.ONE)
			.setSubject(X500Principal("CN=$keyStoreAlias CA Certificate"))
			.setStartDate(startDate.time)
			.setEndDate(endDate.time)

		generator.initialize(builder.build())
	}

	fun removeKey(alias: String) = keyStore.deleteEntry(alias)

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
