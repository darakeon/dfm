package com.darakeon.dfm.auth

import android.annotation.TargetApi
import android.content.Context
import android.os.Build
import android.security.keystore.KeyGenParameterSpec
import android.security.keystore.KeyProperties
import android.util.Base64
import java.math.BigInteger
import java.security.KeyPair
import java.security.KeyPairGenerator
import java.security.KeyStore
import java.security.PrivateKey
import java.util.Calendar
import javax.crypto.BadPaddingException
import javax.crypto.Cipher
import javax.crypto.IllegalBlockSizeException
import javax.security.auth.x500.X500Principal

abstract class BaseAks {
	private val keyStoreType = "AndroidKeyStore"
	protected val keyStoreAlias = "MASTER_KEY"
	private val algorithm = "RSA"
	private val transformationAsymmetric = "RSA/ECB/PKCS1Padding"

	private val keyStore: KeyStore
	private val cipher: Cipher
	private val keyPair: KeyPair

	init {
		keyStore = KeyStore.getInstance(keyStoreType)
		keyStore.load(null)

		cipher = Cipher.getInstance(transformationAsymmetric)

		keyPair = getKeyPair() ?: createKeyPair()
	}

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

		initGenerator(generator)

		return generator.generateKeyPair()
	}

	protected abstract fun initGenerator(
		generator: KeyPairGenerator
	)

	private fun createGenerator() =
		KeyPairGenerator.getInstance(algorithm, keyStoreType)

	fun encrypt(data: String): String {
		if (data == "") return ""

		cipher.init(Cipher.ENCRYPT_MODE, keyPair.public)
		val bytes = cipher.doFinal(data.toByteArray())

		return Base64.encodeToString(bytes, Base64.DEFAULT)
	}

	fun decrypt(data: String): String {
		if (data == "") return ""

		return try {
			cipher.init(Cipher.DECRYPT_MODE, keyPair.private)

			val encryptedData = Base64.decode(data, Base64.DEFAULT)
			val decodedData = cipher.doFinal(encryptedData)

			String(decodedData)
		}
		catch (e: IllegalBlockSizeException) { "" }
		catch (e: BadPaddingException) { "" }
	}
}

class OldAks(val context: Context) : BaseAks() {
	@Suppress("DEPRECATION")
	override fun initGenerator(generator: KeyPairGenerator) {
		val startDate = Calendar.getInstance()
		val endDate = Calendar.getInstance()
		endDate.add(Calendar.YEAR, 20)

		val builder = android.security.KeyPairGeneratorSpec
			.Builder(context)
			.setAlias(keyStoreAlias)
			.setSerialNumber(BigInteger.ONE)
			.setSubject(X500Principal("CN=$keyStoreAlias CA Certificate"))
			.setStartDate(startDate.time)
			.setEndDate(endDate.time)

		generator.initialize(builder.build())
	}
}

@TargetApi(Build.VERSION_CODES.M)
class Aks : BaseAks() {
	override fun initGenerator(generator: KeyPairGenerator) {
		val purpose =
			KeyProperties.PURPOSE_ENCRYPT or
				KeyProperties.PURPOSE_DECRYPT

		val builder = KeyGenParameterSpec.Builder(keyStoreAlias, purpose)
			.setBlockModes(KeyProperties.BLOCK_MODE_ECB)
			.setEncryptionPaddings(KeyProperties.ENCRYPTION_PADDING_RSA_PKCS1)

		generator.initialize(builder.build())
	}
}
