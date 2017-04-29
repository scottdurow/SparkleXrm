// =====================================================================
//
//  This file is part of the Microsoft Dynamics CRM SDK code samples.
//
//  Copyright (C) Microsoft Corporation.  All rights reserved.
//
//  This source code is intended only as a supplement to Microsoft
//  Development Tools and/or on-line documentation.  See these other
//  materials for detailed information regarding Microsoft code samples.
//
//  THIS CODE AND INFORMATION ARE PROVIDED "AS IS" WITHOUT WARRANTY OF ANY
//  KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE
//  IMPLIED WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A
//  PARTICULAR PURPOSE.
//
// =====================================================================
//<snippetDeviceIdManager>
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.IO;
using System.Net;
using System.Runtime.Serialization;
using System.Security.Cryptography;
using System.ServiceModel.Description;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace Microsoft.Crm.Services.Utility
{
	/// <summary>
	/// Management utility for the Device Id
	/// </summary>
	public static class DeviceIdManager
	{
		#region Fields
		private static readonly Random RandomInstance = new Random();

		public const int MaxDeviceNameLength = 24;
		public const int MaxDevicePasswordLength = 24;
		#endregion

		#region Constructor
		static DeviceIdManager()
		{
			PersistToFile = true;
		}
		#endregion

		#region Properties
		/// <summary>
		/// Indicates whether the registered device credentials should be persisted to the database
		/// </summary>
		public static bool PersistToFile { get; set; }

		/// <summary>
		/// Indicates that the credentials should be persisted to the disk if registration fails with DeviceAlreadyExists.
		/// </summary>
		/// <remarks>
		/// If the device already exists, there is a possibility that the credentials are the same as the current credentials that
		/// are being registered. This is especially true in automated environments where the same credentials are used continually (to avoid
		/// registering spurious device credentials.
		/// </remarks>
		public static bool PersistIfDeviceAlreadyExists { get; set; }
		#endregion

		#region Methods
		/// <summary>
		/// Loads the device credentials (if they exist).
		/// </summary>
		/// <returns></returns>
		public static ClientCredentials LoadOrRegisterDevice()
		{
			return LoadOrRegisterDevice(null);
		}

		/// <summary>
		/// Loads the device credentials (if they exist).
		/// </summary>
		/// <param name="deviceName">Device name that should be registered</param>
		/// <param name="devicePassword">Device password that should be registered</param>
		public static ClientCredentials LoadOrRegisterDevice(string deviceName, string devicePassword)
		{
			return LoadOrRegisterDevice(null, deviceName, devicePassword);
		}

		/// <summary>
		/// Loads the device credentials (if they exist).
		/// </summary>
		/// <param name="issuerUri">URL for the current token issuer</param>
		/// <remarks>
		/// The issuerUri can be retrieved from the IServiceConfiguration interface's CurrentIssuer property.
		/// </remarks>
		public static ClientCredentials LoadOrRegisterDevice(Uri issuerUri)
		{
			return LoadOrRegisterDevice(issuerUri, null, null);
		}

		/// <summary>
		/// Loads the device credentials (if they exist).
		/// </summary>
		/// <param name="issuerUri">URL for the current token issuer</param>
		/// <param name="deviceName">Device name that should be registered</param>
		/// <param name="devicePassword">Device password that should be registered</param>
		/// <remarks>
		/// The issuerUri can be retrieved from the IServiceConfiguration interface's CurrentIssuer property.
		/// </remarks>
		public static ClientCredentials LoadOrRegisterDevice(Uri issuerUri, string deviceName, string devicePassword)
		{
			ClientCredentials credentials = LoadDeviceCredentials(issuerUri);
			if (null == credentials)
			{
				credentials = RegisterDevice(Guid.NewGuid(), issuerUri, deviceName, devicePassword);
			}

			return credentials;
		}

		/// <summary>
		/// Registers the given device with Microsoft account with a random application ID
		/// </summary>
		/// <returns>ClientCredentials that were registered</returns>
		public static ClientCredentials RegisterDevice()
		{
			return RegisterDevice(Guid.NewGuid());
		}

		/// <summary>
		/// Registers the given device with Microsoft account
		/// </summary>
		/// <param name="applicationId">ID for the application</param>
		/// <returns>ClientCredentials that were registered</returns>
		public static ClientCredentials RegisterDevice(Guid applicationId)
		{
			return RegisterDevice(applicationId, (Uri)null);
		}

		/// <summary>
		/// Registers the given device with Microsoft account
		/// </summary>
		/// <param name="applicationId">ID for the application</param>
		/// <param name="issuerUri">URL for the current token issuer</param>
		/// <returns>ClientCredentials that were registered</returns>
		/// <remarks>
		/// The issuerUri can be retrieved from the IServiceConfiguration interface's CurrentIssuer property.
		/// </remarks>
		public static ClientCredentials RegisterDevice(Guid applicationId, Uri issuerUri)
		{
			return RegisterDevice(applicationId, issuerUri, null, null);
		}

		/// <summary>
		/// Registers the given device with Microsoft account
		/// </summary>
		/// <param name="applicationId">ID for the application</param>
		/// <param name="deviceName">Device name that should be registered</param>
		/// <param name="devicePassword">Device password that should be registered</param>
		/// <returns>ClientCredentials that were registered</returns>
		public static ClientCredentials RegisterDevice(Guid applicationId, string deviceName, string devicePassword)
		{
			return RegisterDevice(applicationId, (Uri)null, deviceName, devicePassword);
		}

		/// <summary>
		/// Registers the given device with Microsoft account
		/// </summary>
		/// <param name="applicationId">ID for the application</param>
		/// <param name="issuerUri">URL for the current token issuer</param>
		/// <param name="deviceName">Device name that should be registered</param>
		/// <param name="devicePassword">Device password that should be registered</param>
		/// <returns>ClientCredentials that were registered</returns>
		/// <remarks>
		/// The issuerUri can be retrieved from the IServiceConfiguration interface's CurrentIssuer property.
		/// </remarks>
		public static ClientCredentials RegisterDevice(Guid applicationId, Uri issuerUri, string deviceName, string devicePassword)
		{
			if (string.IsNullOrEmpty(deviceName) && !PersistToFile)
			{
				throw new ArgumentNullException("deviceName", "If PersistToFile is false, then deviceName must be specified.");
			}
			else if (string.IsNullOrEmpty(deviceName) != string.IsNullOrEmpty(devicePassword))
			{
				throw new ArgumentNullException("deviceName", "Either deviceName/devicePassword should both be specified or they should be null.");
			}

			LiveDevice device = GenerateDevice(deviceName, devicePassword);
			return RegisterDevice(applicationId, issuerUri, device);
		}

		/// <summary>
		/// Loads the device's credentials from the file system
		/// </summary>
		/// <returns>Device Credentials (if set) or null</returns>
		public static ClientCredentials LoadDeviceCredentials()
		{
			return LoadDeviceCredentials(null);
		}

		/// <summary>
		/// Loads the device's credentials from the file system
		/// </summary>
		/// <param name="issuerUri">URL for the current token issuer</param>
		/// <returns>Device Credentials (if set) or null</returns>
		/// <remarks>
		/// The issuerUri can be retrieved from the IServiceConfiguration interface's CurrentIssuer property.
		/// </remarks>
		public static ClientCredentials LoadDeviceCredentials(Uri issuerUri)
		{
			//If the credentials should not be persisted to a file, then they won't be present on the disk.
			if (!PersistToFile)
			{
				return null;
			}

			EnvironmentConfiguration environment = DiscoverEnvironmentInternal(issuerUri);

			LiveDevice device = ReadExistingDevice(environment);
			if (null == device || null == device.User)
			{
				return null;
			}

			return device.User.ToClientCredentials();
		}

		/// <summary>
		/// Discovers the Microsoft account environment based on the Token Issuer
		/// </summary>
		public static string DiscoverEnvironment(Uri issuerUri)
		{
			return DiscoverEnvironmentInternal(issuerUri).Environment;
		}
		#endregion

		#region Private Methods
		private static EnvironmentConfiguration DiscoverEnvironmentInternal(Uri issuerUri)
		{
			if (null == issuerUri)
			{
				return new EnvironmentConfiguration(EnvironmentType.LiveDeviceID, "login.live.com", null);
			}

			Dictionary<EnvironmentType, string> searchList = new Dictionary<EnvironmentType, string>();
			searchList.Add(EnvironmentType.LiveDeviceID, "login.live");
			searchList.Add(EnvironmentType.OrgDeviceID, "login.microsoftonline");

			foreach (KeyValuePair<EnvironmentType, string> searchPair in searchList)
			{
				if (issuerUri.Host.Length > searchPair.Value.Length &&
					issuerUri.Host.StartsWith(searchPair.Value, StringComparison.OrdinalIgnoreCase))
				{
					string environment = issuerUri.Host.Substring(searchPair.Value.Length);

					//Parse out the environment
					if ('-' == environment[0])
					{
						int separatorIndex = environment.IndexOf('.', 1);
						if (-1 != separatorIndex)
						{
							environment = environment.Substring(1, separatorIndex - 1);
						}
						else
						{
							environment = null;
						}
					}
					else
					{
						environment = null;
					}

					return new EnvironmentConfiguration(searchPair.Key, issuerUri.Host, environment);
				}
			}

			//In all other cases the environment is either not applicable or it is a production system
			return new EnvironmentConfiguration(EnvironmentType.LiveDeviceID, issuerUri.Host, null);
		}

		private static void Serialize<T>(Stream stream, T value)
		{
			XmlSerializer serializer = new XmlSerializer(typeof(T), string.Empty);

			XmlSerializerNamespaces xmlNamespaces = new XmlSerializerNamespaces();
			xmlNamespaces.Add(string.Empty, string.Empty);

			serializer.Serialize(stream, value, xmlNamespaces);
		}

		private static T Deserialize<T>(string operationName, Stream stream)
		{
			//Read the XML into memory so that the data can be used in an exception if necessary
			using (StreamReader reader = new StreamReader(stream))
			{
				return Deserialize<T>(operationName, reader.ReadToEnd());
			}
		}

		private static T Deserialize<T>(string operationName, string xml)
		{
			//Attempt to deserialize the data. If deserialization fails, include the XML in the exception that is thrown for further
			//investigation
			using (StringReader reader = new StringReader(xml))
			{
				try
				{
					XmlSerializer serializer = new XmlSerializer(typeof(T), string.Empty);
					return (T)serializer.Deserialize(reader);
				}
				catch (InvalidOperationException ex)
				{
					throw new InvalidOperationException(string.Format(CultureInfo.InvariantCulture,
						"Unable to Deserialize XML (Operation = {0}):{1}{2}", operationName, Environment.NewLine, xml), ex);
				}
			}
		}

		private static FileInfo GetDeviceFile(EnvironmentConfiguration environment)
		{
			return new FileInfo(string.Format(CultureInfo.InvariantCulture, LiveIdConstants.FileNameFormat,
				environment.Type,
				string.IsNullOrEmpty(environment.Environment) ? null : "-" + environment.Environment.ToUpperInvariant()));
		}

		private static ClientCredentials RegisterDevice(Guid applicationId, Uri issuerUri, LiveDevice device)
		{
			EnvironmentConfiguration environment = DiscoverEnvironmentInternal(issuerUri);

			DeviceRegistrationRequest request = new DeviceRegistrationRequest(applicationId, device);

			string url = string.Format(CultureInfo.InvariantCulture, LiveIdConstants.RegistrationEndpointUriFormat,
				environment.HostName);

			DeviceRegistrationResponse response = ExecuteRegistrationRequest(url, request);
			if (!response.IsSuccess)
			{
				bool throwException = true;
				if (DeviceRegistrationErrorCode.DeviceAlreadyExists == response.Error.RegistrationErrorCode)
				{
					if (!PersistToFile)
					{
						//If the file is not persisted, the registration will always occur (since the credentials are not
						//persisted to the disk. However, the credentials may already exist. To avoid an exception being continually
						//processed by the calling user, DeviceAlreadyExists will be ignored if the credentials are not persisted to the disk.
						return device.User.ToClientCredentials();
					}
					else if (PersistIfDeviceAlreadyExists)
					{
						// This flag indicates that the 
						throwException = false;
					}
				}

				if (throwException)
				{
					throw new DeviceRegistrationFailedException(response.Error.RegistrationErrorCode, response.ErrorSubCode);
				}
			}

			if (PersistToFile || PersistIfDeviceAlreadyExists)
			{
				WriteDevice(environment, device);
			}

			return device.User.ToClientCredentials();
		}

		private static LiveDevice GenerateDevice(string deviceName, string devicePassword)
		{
			// If the deviceName hasn't been specified, it should be generated using random characters.
			DeviceUserName userNameCredentials;
			if (string.IsNullOrEmpty(deviceName))
			{
				userNameCredentials = GenerateDeviceUserName();
			}
			else
			{
				userNameCredentials = new DeviceUserName() { DeviceName = deviceName, DecryptedPassword = devicePassword };
			}

			return new LiveDevice() { User = userNameCredentials, Version = 1 };
		}

		private static LiveDevice ReadExistingDevice(EnvironmentConfiguration environment)
		{
			//Retrieve the file info
			FileInfo file = GetDeviceFile(environment);
			if (!file.Exists)
			{
				return null;
			}

			using (FileStream stream = file.Open(FileMode.Open, FileAccess.Read, FileShare.Read))
			{
				return Deserialize<LiveDevice>("Loading Device Credentials from Disk", stream);
			}
		}

		private static void WriteDevice(EnvironmentConfiguration environment, LiveDevice device)
		{
			FileInfo file = GetDeviceFile(environment);
			if (!file.Directory.Exists)
			{
				file.Directory.Create();
			}

			using (FileStream stream = file.Open(FileMode.CreateNew, FileAccess.Write, FileShare.None))
			{
				Serialize(stream, device);
			}
		}

		private static DeviceRegistrationResponse ExecuteRegistrationRequest(string url, DeviceRegistrationRequest registrationRequest)
		{
			//Create the request that will submit the request to the server
			WebRequest request = WebRequest.Create(url);
			request.ContentType = "application/soap+xml; charset=UTF-8";
			request.Method = "POST";
			request.Timeout = 180000;

			//Write the envelope to the RequestStream
			using (Stream stream = request.GetRequestStream())
			{
				Serialize(stream, registrationRequest);
			}

			// Read the response into an XmlDocument and return that doc
			try
			{
				using (WebResponse response = request.GetResponse())
				{
					using (Stream stream = response.GetResponseStream())
					{
						return Deserialize<DeviceRegistrationResponse>("Deserializing Registration Response", stream);
					}
				}
			}
			catch (WebException ex)
			{
				System.Diagnostics.Trace.TraceError("Microsoft account Device Registration Failed (HTTP Code: {0}): {1}",
					ex.Status, ex.Message);

				if (null != ex.Response)
				{
					using (Stream stream = ex.Response.GetResponseStream())
					{
						return Deserialize<DeviceRegistrationResponse>("Deserializing Failed Registration Response", stream);
					}
				}

				throw;
			}
		}

		private static DeviceUserName GenerateDeviceUserName()
		{
			DeviceUserName userName = new DeviceUserName();
			userName.DeviceName = GenerateRandomString(LiveIdConstants.ValidDeviceNameCharacters, MaxDeviceNameLength);
			userName.DecryptedPassword = GenerateRandomString(LiveIdConstants.ValidDevicePasswordCharacters, MaxDevicePasswordLength);

			return userName;
		}

		private static string GenerateRandomString(string characterSet, int count)
		{
			//Create an array of the characters that will hold the final list of random characters
			char[] value = new char[count];

			//Convert the character set to an array that can be randomly accessed
			char[] set = characterSet.ToCharArray();

			lock (RandomInstance)
			{
				//Populate the array with random characters from the character set
				for (int i = 0; i < count; i++)
				{
					value[i] = set[RandomInstance.Next(0, set.Length)];
				}
			}

			return new string(value);
		}
		#endregion

		#region Private Classes
		private enum EnvironmentType
		{
			LiveDeviceID,
			OrgDeviceID
		}

		private sealed class EnvironmentConfiguration
		{
			public EnvironmentConfiguration(EnvironmentType type, string hostName, string environment)
			{
				if (string.IsNullOrWhiteSpace(hostName))
				{
					throw new ArgumentNullException("hostName");
				}

				this.Type = type;
				this.HostName = hostName;
				this.Environment = environment;
			}

			#region Properties
			public EnvironmentType Type { get; private set; }

			public string HostName { get; private set; }

			public string Environment { get; private set; }
			#endregion
		}

		private static class LiveIdConstants
		{
			public const string RegistrationEndpointUriFormat = @"https://{0}/ppsecure/DeviceAddCredential.srf";

			public static readonly string FileNameFormat = Path.Combine(
				Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "LiveDeviceID"),
				"{0}{1}.xml");

			public const string ValidDeviceNameCharacters = "0123456789abcdefghijklmnopqrstuvqxyz";

			//Consists of the list of characters specified in the documentation
			public const string ValidDevicePasswordCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789!@#$%^*()-_=+;,./?`~";
		}
		#endregion
	}

	#region Public Classes & Enums
	/// <summary>
	/// Indicates an error during registration
	/// </summary>
	public enum DeviceRegistrationErrorCode
	{
		/// <summary>
		/// Unspecified or Unknown Error occurred
		/// </summary>
		Unknown = 0,

		/// <summary>
		/// Interface Disabled
		/// </summary>
		InterfaceDisabled = 1,

		/// <summary>
		/// Invalid Request Format
		/// </summary>
		InvalidRequestFormat = 3,

		/// <summary>
		/// Unknown Client Version
		/// </summary>
		UnknownClientVersion = 4,

		/// <summary>
		/// Blank Password
		/// </summary>
		BlankPassword = 6,

		/// <summary>
		/// Missing Device User Name or Password
		/// </summary>
		MissingDeviceUserNameOrPassword = 7,

		/// <summary>
		/// Invalid Parameter Syntax
		/// </summary>
		InvalidParameterSyntax = 8,

		/// <summary>
		/// Invalid Characters are used in the device credentials.
		/// </summary>
		InvalidCharactersInCredentials = 9,

		/// <summary>
		/// Internal Error
		/// </summary>
		InternalError = 11,

		/// <summary>
		/// Device Already Exists
		/// </summary>
		DeviceAlreadyExists = 13
	}

	/// <summary>
	/// Indicates that Device Registration failed
	/// </summary>
	[Serializable]
	public sealed class DeviceRegistrationFailedException : Exception
	{
		/// <summary>
		/// Construct an instance of the DeviceRegistrationFailedException class
		/// </summary>
		public DeviceRegistrationFailedException()
			: base()
		{
		}

		/// <summary>
		/// Construct an instance of the DeviceRegistrationFailedException class
		/// </summary>
		/// <param name="message">Message to pass</param>
		public DeviceRegistrationFailedException(string message)
			: base(message)
		{
		}

		/// <summary>
		/// Construct an instance of the DeviceRegistrationFailedException class
		/// </summary>
		/// <param name="message">Message to pass</param>
		/// <param name="innerException">Exception to include</param>
		public DeviceRegistrationFailedException(string message, Exception innerException)
			: base(message, innerException)
		{
		}

		/// <summary>
		/// Construct an instance of the DeviceRegistrationFailedException class
		/// </summary>
		/// <param name="code">Error code that occurred</param>
		/// <param name="subCode">Subcode that occurred</param>
		public DeviceRegistrationFailedException(DeviceRegistrationErrorCode code, string subCode)
			: this(code, subCode, null)
		{
		}

		/// <summary>
		/// Construct an instance of the DeviceRegistrationFailedException class
		/// </summary>
		/// <param name="code">Error code that occurred</param>
		/// <param name="subCode">Subcode that occurred</param>
		/// <param name="innerException">Inner exception</param>
		public DeviceRegistrationFailedException(DeviceRegistrationErrorCode code, string subCode, Exception innerException)
			: base(string.Concat(code.ToString(), ": ", subCode), innerException)
		{
			this.RegistrationErrorCode = code;
		}

		/// <summary>
		/// Construct an instance of the DeviceRegistrationFailedException class
		/// </summary>
		/// <param name="si"></param>
		/// <param name="sc"></param>
		private DeviceRegistrationFailedException(SerializationInfo si, StreamingContext sc)
			: base(si, sc)
		{
		}

		#region Properties
		/// <summary>
		/// Error code that occurred during registration
		/// </summary>
		public DeviceRegistrationErrorCode RegistrationErrorCode { get; private set; }
		#endregion

		#region Methods
		public override void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			base.GetObjectData(info, context);
		}
		#endregion
	}

	#region Serialization Classes
	#region DeviceRegistrationRequest Class
	[EditorBrowsable(EditorBrowsableState.Never)]
	[XmlRoot("DeviceAddRequest")]
	public sealed class DeviceRegistrationRequest
	{
		#region Constructors
		public DeviceRegistrationRequest()
		{
		}

		public DeviceRegistrationRequest(Guid applicationId, LiveDevice device)
			: this()
		{
			if (null == device)
			{
				throw new ArgumentNullException("device");
			}

			this.ClientInfo = new DeviceRegistrationClientInfo() { ApplicationId = applicationId, Version = "1.0" };
			this.Authentication = new DeviceRegistrationAuthentication()
			{
				MemberName = device.User.DeviceId,
				Password = device.User.DecryptedPassword
			};
		}
		#endregion

		#region Properties
		[XmlElement("ClientInfo")]
		public DeviceRegistrationClientInfo ClientInfo { get; set; }

		[XmlElement("Authentication")]
		public DeviceRegistrationAuthentication Authentication { get; set; }
		#endregion
	}
	#endregion

	#region DeviceRegistrationClientInfo Class
	[EditorBrowsable(EditorBrowsableState.Never)]
	[XmlRoot("ClientInfo")]
	public sealed class DeviceRegistrationClientInfo
	{
		#region Properties
		[XmlAttribute("name")]
		public Guid ApplicationId { get; set; }

		[XmlAttribute("version")]
		public string Version { get; set; }
		#endregion
	}
	#endregion

	#region DeviceRegistrationAuthentication Class
	[EditorBrowsable(EditorBrowsableState.Never)]
	[XmlRoot("Authentication")]
	public sealed class DeviceRegistrationAuthentication
	{
		#region Properties
		[XmlElement("Membername")]
		public string MemberName { get; set; }

		[XmlElement("Password")]
		public string Password { get; set; }
		#endregion
	}
	#endregion

	#region DeviceRegistrationResponse Class
	[EditorBrowsable(EditorBrowsableState.Never)]
	[XmlRoot("DeviceAddResponse")]
	public sealed class DeviceRegistrationResponse
	{
		#region Properties
		[XmlElement("success")]
		public bool IsSuccess { get; set; }

		[XmlElement("puid")]
		public string Puid { get; set; }

		[XmlElement("Error")]
		public DeviceRegistrationResponseError Error { get; set; }

		[XmlElement("ErrorSubcode")]
		public string ErrorSubCode { get; set; }
		#endregion
	}
	#endregion

	#region DeviceRegistrationResponse Class
	[EditorBrowsable(EditorBrowsableState.Never)]
	[XmlRoot("Error")]
	public sealed class DeviceRegistrationResponseError
	{
		private string _code;

		#region Properties
		[XmlAttribute("Code")]
		public string Code
		{
			get
			{
				return this._code;
			}

			set
			{
				this._code = value;

				//Parse the error code
				if (!string.IsNullOrEmpty(value))
				{
					//Parse the error code
					if (value.StartsWith("dc", StringComparison.Ordinal))
					{
						int code;
						if (int.TryParse(value.Substring(2), NumberStyles.Integer,
							CultureInfo.InvariantCulture, out code) &&
							Enum.IsDefined(typeof(DeviceRegistrationErrorCode), code))
						{
							this.RegistrationErrorCode = (DeviceRegistrationErrorCode)Enum.ToObject(
								typeof(DeviceRegistrationErrorCode), code);
						}
					}
				}
			}
		}

		[XmlIgnore]
		public DeviceRegistrationErrorCode RegistrationErrorCode { get; private set; }
		#endregion
	}
	#endregion

	#region LiveDevice Class
	[EditorBrowsable(EditorBrowsableState.Never)]
	[XmlRoot("Data")]
	public sealed class LiveDevice
	{
		#region Properties
		[XmlAttribute("version")]
		public int Version { get; set; }

		[XmlElement("User")]
		public DeviceUserName User { get; set; }

		[SuppressMessage("Microsoft.Design", "CA1059:MembersShouldNotExposeCertainConcreteTypes", MessageId = "System.Xml.XmlNode", Justification = "This is required for proper XML Serialization")]
		[XmlElement("Token")]
		public XmlNode Token { get; set; }

		[XmlElement("Expiry")]
		public string Expiry { get; set; }

		[XmlElement("ClockSkew")]
		public string ClockSkew { get; set; }
		#endregion
	}
	#endregion

	#region DeviceUserName Class
	[EditorBrowsable(EditorBrowsableState.Never)]
	public sealed class DeviceUserName
	{
		private string _encryptedPassword;
		private string _decryptedPassword;
		private bool _encryptedValueIsUpdated;

		#region Constants
		private const string UserNamePrefix = "11";
		#endregion

		#region Constructors
		public DeviceUserName()
		{
			this.UserNameType = "Logical";
		}
		#endregion

		#region Properties
		[XmlAttribute("username")]
		public string DeviceName { get; set; }

		[XmlAttribute("type")]
		public string UserNameType { get; set; }

		[XmlElement("Pwd")]
		public string EncryptedPassword
		{
			get
			{
				this.ThrowIfNoEncryption();

				if (!this._encryptedValueIsUpdated)
				{
					this._encryptedPassword = this.Encrypt(this._decryptedPassword);
					this._encryptedValueIsUpdated = true;
				}

				return this._encryptedPassword;
			}

			set
			{
				this.ThrowIfNoEncryption();
				this.UpdateCredentials(value, null);
			}
		}

		public string DeviceId
		{
			get
			{
				return UserNamePrefix + DeviceName;
			}
		}

		[XmlIgnore]
		public string DecryptedPassword
		{
			get
			{
				return this._decryptedPassword;
			}

			set
			{
				this.UpdateCredentials(null, value);
			}
		}

		private bool IsEncryptionEnabled
		{
			get
			{
				//If the object is not going to be persisted to a file, then the value does not need to be encrypted. This is extra
				//overhead and will not function in partial trust.
				return DeviceIdManager.PersistToFile;
			}
		}
		#endregion

		#region Methods
		public ClientCredentials ToClientCredentials()
		{
			ClientCredentials credentials = new ClientCredentials();
			credentials.UserName.UserName = this.DeviceId;
			credentials.UserName.Password = this.DecryptedPassword;

			return credentials;
		}

		private void ThrowIfNoEncryption()
		{
			if (!this.IsEncryptionEnabled)
			{
				throw new NotSupportedException("Not supported when DeviceIdManager.UseEncryptionApis is false.");
			}
		}

		private void UpdateCredentials(string encryptedValue, string decryptedValue)
		{
			bool isValueUpdated = false;
			if (string.IsNullOrEmpty(encryptedValue) && string.IsNullOrEmpty(decryptedValue))
			{
				isValueUpdated = true;
			}
			else if (string.IsNullOrEmpty(encryptedValue))
			{
				if (this.IsEncryptionEnabled)
				{
					encryptedValue = this.Encrypt(decryptedValue);
					isValueUpdated = true;
				}
				else
				{
					encryptedValue = null;
					isValueUpdated = false;
				}
			}
			else
			{
				this.ThrowIfNoEncryption();

				decryptedValue = this.Decrypt(encryptedValue);
				isValueUpdated = true;
			}

			this._encryptedPassword = encryptedValue;
			this._decryptedPassword = decryptedValue;
			this._encryptedValueIsUpdated = isValueUpdated;
		}

		private string Encrypt(string value)
		{
			if (string.IsNullOrEmpty(value))
			{
				return value;
			}

			byte[] encryptedBytes = ProtectedData.Protect(Encoding.UTF8.GetBytes(value), null, DataProtectionScope.CurrentUser);
			return Convert.ToBase64String(encryptedBytes);
		}

		private string Decrypt(string value)
		{
			if (string.IsNullOrEmpty(value))
			{
				return value;
			}

			byte[] decryptedBytes = ProtectedData.Unprotect(Convert.FromBase64String(value), null, DataProtectionScope.CurrentUser);
			if (null == decryptedBytes || 0 == decryptedBytes.Length)
			{
				return null;
			}

			return Encoding.UTF8.GetString(decryptedBytes, 0, decryptedBytes.Length);
		}
		#endregion
	}
	#endregion
	#endregion
	#endregion
}
//</snippetDeviceIdManager>
