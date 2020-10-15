using System;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Security.Cryptography;
using System.Text;

namespace CRM.Database
{
    public static class Utility
    {
        /*
                public static byte[] GetBytesFromASCIIString(string asciiInput)
                {
                    byte[] sendBytesout = null;
                    try
                    {
                        var sendBytes = Encoding.ASCII.GetBytes(asciiInput);
                        asciiInput = BitConverter.ToString(sendBytes, 0, asciiInput.Length);
                        asciiInput = "00" + "-" + "C" + "-" + asciiInput;
                        asciiInput = asciiInput.Replace("7C", "1C");
                        var hexvaluesplit = asciiInput.Split('-');
                        var stringval = hexvaluesplit.Select(hx => Convert.ToInt32(hx, 16)).Aggregate("", (current, val) => current + char.ConvertFromUtf32(val));
                        sendBytesout = Encoding.ASCII.GetBytes(stringval);
                        //i = networkStream.Read(sendBytesout, 0, sendBytesout.Length);
                    }
                    catch (Exception)
                    {
                        //ex.ToString();
                    }
                    return sendBytesout;
                }
        */
/*
        private const string PRIVATE_KEY = "A1B2C3D4E5F6G7H8"; //I9J0K1L2M3N4O5P6
*/
        /*
                public static string ConvertDecimalToHex(string inputInDecimal)
                {
                    //string str = "\0\f12001R009";
                    char[] charValues = inputInDecimal.ToCharArray();
                    string hexOutput = "";
                    foreach (char _eachChar in charValues)
                    {
                        // Get the integral value of the character.
                        int value = Convert.ToInt32(_eachChar);
                        // Convert the decimal value to a hexadecimal value in string form.
                        hexOutput += String.Format("{0:X}", value);
                        // to make output as your eg 
                        //  hexOutput +=" "+ String.Format("{0:X}", value);


                    }
                    return hexOutput;

                }
        */
        public const string PRIVATE_KEY = "A1B2C3D4E5F6G7H8";
        public static string ConvertDecimalToHex2(string inputInDecimal)
        {
            //string str = "\0\f12001R009";
            //char[] charValues = inputInDecimal.ToCharArray();
            long inDecimal;
            long.TryParse(inputInDecimal, out inDecimal);
            var hexOutput = string.Format("{0:X}", inDecimal);
            //foreach (char _eachChar in charValues)
            //{
            // Get the integral value of the character.
            //int value = Convert.ToInt32(_eachChar);
            // Convert the decimal value to a hexadecimal value in string form.
            //hexOutput += String.Format("{0:X}", value);
            // to make output as your eg 
            //  hexOutput +=" "+ String.Format("{0:X}", value);


            //}
            return hexOutput;

        }

        /*
                public static string convertvaltoascii(byte[] datafromclient, int i)
                {

                    try
                    {
                        string stringvluesofbyte = BitConverter.ToString(datafromclient, 0, i);
                        stringvluesofbyte = stringvluesofbyte.Replace("1C", "7C");
                        // stringvluesofbyte = stringvluesofbyte.Replace("-", " ");
                        string[] hexvaluesplit = stringvluesofbyte.Split('-');
                        string stringval = "";

                        foreach (string hx in hexvaluesplit)
                        {
                            int val = Convert.ToInt32(hx, 16);
                            stringval = stringval + char.ConvertFromUtf32(val);

                        }// foreach end
                        return stringval;
                    }
                    catch (Exception ex)
                    {
                        return ex.ToString();
                    }
                }
        */

        public static byte[] GetBytesWithMetaLength(string Msg)
        {


            var sendBytesout = new byte[10025];
            var inservice = Msg;
            var sendBytes = Encoding.ASCII.GetBytes(inservice);

            var inlength = inservice.Length;
            var div = inlength / 256;
            var Per = inlength % 256;


            var byte1 = div.ToString("X");
            var byte2 = Per.ToString("X");
            //string msgx_len = inlength.ToString("X");
            inservice = BitConverter.ToString(sendBytes, 0, inservice.Length);
            //= simile step
            // inservice = byte1 + "-" + byte2 + "-" + inservice;
            inservice = inservice.Replace("7C", "1C");
            inservice = inservice.Replace("2A", "1D");
            //inservice = inservice.Replace("3b", "6");
            inservice = byte1 + "-" + byte2 + "-" + inservice;

            var n = 0;
            var hexvaluesplit = inservice.Split('-');
            foreach (var hx in hexvaluesplit)
            {
                var val = Convert.ToInt32(hx, 16);
                //stringval = stringval + char.ConvertFromUtf32(val);
                sendBytesout[n] = (byte)val;
                n++;
            }

            //  int ale =sendBytesout.Length;

            //sendBytesout = Encoding.ASCII.GetBytes(stringval);


            Array.Resize(ref sendBytesout, n);
            // sendBytesout[sendBytesout.GetUpperBound(0)] = sendBytesout.GetUpperBound(0);
            return sendBytesout;





        }

        /*
                public static string ReplaceDelimiter(byte[] incommsg, int length)
                {
                    string str = BitConverter.ToString(incommsg, 0, length);
                    str = str.Replace("1C", "7C");

                    string[] hexvaluesplit = str.Split('-');
                    string stringval = "";
                    foreach (string hx in hexvaluesplit)
                    {
                        int val = Convert.ToInt32(hx, 16);
                        stringval = stringval + char.ConvertFromUtf32(val);

                    }
                    return stringval;

                }
        */

        // Hexa decimal to binary
        public static string ConvertHexToBinary(string hexValue)
        {
            var binaryval = Convert.ToString(Convert.ToInt32(hexValue, 16), 2);
            return binaryval.PadLeft(8, '0');
        }

        // Hexa hex to decimal
        public static long ConvertHexToDecimal(string hexValue)
        {
            hexValue = hexValue.Replace("x", string.Empty);
            long result;
            long.TryParse(hexValue, System.Globalization.NumberStyles.HexNumber, null, out result);
            return result;
        }

        // binary to hex
        public static string ConvertBinaryToHex(string binaryValue)
        {
            var result = new StringBuilder(binaryValue.Length / 8 + 1);

            // TODO: check all 1's or 0's... Will throw otherwise

            var mod4Len = binaryValue.Length % 8;
            if (mod4Len != 0)
            {
                // pad to length multiple of 8
                binaryValue = binaryValue.PadLeft((binaryValue.Length / 8 + 1) * 8, '0');
            }

            for (var i = 0; i < binaryValue.Length; i += 8)
            {
                var eightBits = binaryValue.Substring(i, 8);
                result.AppendFormat("{0:X2}", Convert.ToByte(eightBits, 2));
            }

            return result.ToString();
        }

        public static string Hextobinary(string hexNumber)
        {
            var binarystring = string.Join(string.Empty,
                hexNumber.Select(
                c => Convert.ToString(Convert.ToInt32(c.ToString(), 16), 2).PadLeft(4, '0')
                )
                );
            return binarystring;
        }

        /*
                public static string ByteArrayToString(byte[] ba)
                {
                    var hex = BitConverter.ToString(ba);
                    return hex.Replace("-", "");
                }
        */

        /*
                public static byte[] StringToByteArray(String hex)
                {
                    var NumberChars = hex.Length;
                    var bytes = new byte[NumberChars / 2];
                    for (var i = 0; i < NumberChars; i += 2)
                        bytes[i / 2] = Convert.ToByte(hex.Substring(i, 2), 16);
                    return bytes;
                }
        */
        //reverce string
        /*
                public static string Reverse(string str)
                {
                    var charArray = str.ToCharArray();
                    Array.Reverse(charArray);
                    return new string(charArray);
                }
        */
        public static string RemoveSpecialCharacters(string str)
        {
            var sb = new StringBuilder();

            foreach (var c in str)
            {
                if (c >= '0' && c <= '9' || c >= 'A' && c <= 'Z' || c >= 'a' && c <= 'z' || c == '.')
                {
                    sb.Append(c);
                }
            }
            return sb.ToString().ToUpper();
        }

        /*
                public static DateTime ResetTimeToStartOfDay(this DateTime dateTime)
                {
                    return new DateTime(
                       dateTime.Year,
                       dateTime.Month,
                       dateTime.Day,
                       0, 0, 0, 0);
                }
        */

/*
        public static string Encrypt(string plainText)
        {
            byte[] keyArray;
            byte[] toEncryptArray = UTF8Encoding.UTF8.GetBytes(plainText);

            keyArray = UTF8Encoding.UTF8.GetBytes(PRIVATE_KEY);

            AesManaged tdes = new AesManaged();
            //set the secret key for the tripleDES algorithm
            tdes.Key = keyArray;
            //mode of operation. there are other 4 modes. We choose ECB(Electronic code Book)
            tdes.Mode = CipherMode.ECB;
            //padding mode(if any extra byte added)
            tdes.Padding = PaddingMode.PKCS7;

            ICryptoTransform cTransform = tdes.CreateEncryptor();
            //transform the specified region of bytes array to resultArray
            byte[] resultArray = cTransform.TransformFinalBlock
                    (toEncryptArray, 0, toEncryptArray.Length);
            //Release resources held by TripleDes Encryptor
            tdes.Clear();
            //Return the encrypted data into unreadable string format
            return Convert.ToBase64String(resultArray, 0, resultArray.Length);

        }
*/
        public static string Encrypt(string plainText)
        {
            byte[] keyArray;
            byte[] toEncryptArray = UTF8Encoding.UTF8.GetBytes(plainText);

            keyArray = UTF8Encoding.UTF8.GetBytes(PRIVATE_KEY);

            TripleDESCryptoServiceProvider tdes = new TripleDESCryptoServiceProvider();
            //set the secret key for the tripleDES algorithm
            tdes.Key = keyArray;
            //mode of operation. there are other 4 modes. We choose ECB(Electronic code Book)
            tdes.Mode = CipherMode.ECB;
            //padding mode(if any extra byte added)
            tdes.Padding = PaddingMode.PKCS7;

            ICryptoTransform cTransform = tdes.CreateEncryptor();
            //transform the specified region of bytes array to resultArray
            byte[] resultArray = cTransform.TransformFinalBlock
                    (toEncryptArray, 0, toEncryptArray.Length);
            //Release resources held by TripleDes Encryptor
            tdes.Clear();
            //Return the encrypted data into unreadable string format
            return Convert.ToBase64String(resultArray, 0, resultArray.Length);

        }

        public static string Decrypt(string cypherText)
        {

            byte[] keyArray;

            //get the byte code of the string
            byte[] toEncryptArray = Convert.FromBase64String(cypherText);


            keyArray = UTF8Encoding.UTF8.GetBytes(PRIVATE_KEY);

            TripleDESCryptoServiceProvider tdes = new TripleDESCryptoServiceProvider();
            //set the secret key for the tripleDES algorithm
            tdes.Key = keyArray;
            //mode of operation. there are other 4 modes.
            //We choose ECB(Electronic code Book)

            tdes.Mode = CipherMode.ECB;
            //padding mode(if any extra byte added)
            tdes.Padding = PaddingMode.PKCS7;

            ICryptoTransform cTransform = tdes.CreateDecryptor();
            byte[] resultArray = cTransform.TransformFinalBlock
                    (toEncryptArray, 0, toEncryptArray.Length);
            //Release resources held by TripleDes Encryptor
            tdes.Clear();
            //return the Clear decrypted TEXT
            return UTF8Encoding.UTF8.GetString(resultArray);

        }


/*
        public static string Decrypt(string cypherText)
        {
            //get the byte code of the string
            byte[] toEncryptArray = Convert.FromBase64String(cypherText);


            var keyArray = UTF8Encoding.UTF8.GetBytes(PRIVATE_KEY);

            TripleDESCryptoServiceProvider tdes = new TripleDESCryptoServiceProvider();
            //set the secret key for the tripleDES algorithm
            tdes.Key = keyArray;
            //mode of operation. there are other 4 modes.
            //We choose ECB(Electronic code Book)

            tdes.Mode = CipherMode.ECB;
            //padding mode(if any extra byte added)
            tdes.Padding = PaddingMode.PKCS7;

            ICryptoTransform cTransform = tdes.CreateDecryptor();
            byte[] resultArray = cTransform.TransformFinalBlock
                    (toEncryptArray, 0, toEncryptArray.Length);
            //Release resources held by TripleDes Encryptor
            tdes.Clear();
            //return the Clear decrypted TEXT
            return UTF8Encoding.UTF8.GetString(resultArray);

        }
*/

        public static string Encrypt(string plainText, string key)
        {
            byte[] keyArray;
            var toEncryptArray = Encoding.UTF8.GetBytes(plainText);

            //System.Configuration.AppSettingsReader settingsReader =
            //                                    new AppSettingsReader();
            // Get the key from config file

            //string key = (string)settingsReader.GetValue(key,
            //                                                 typeof(String));
            //System.Windows.Forms.MessageBox.Show(key);
            //If hashing use get hashcode regards to your key
            if (true)
            {
                var hashmd5 = new MD5CryptoServiceProvider();
                keyArray = hashmd5.ComputeHash(Encoding.UTF8.GetBytes(key));
                //Always release the resources and flush data
                // of the Cryptographic service provide. Best Practice

                hashmd5.Clear();
            }
            //else
            //    keyArray = UTF8Encoding.UTF8.GetBytes(key);

            var tdes = new TripleDESCryptoServiceProvider
            {
                Key = keyArray,
                Mode = CipherMode.ECB,
                Padding = PaddingMode.PKCS7
            };
            //set the secret key for the tripleDES algorithm
            //mode of operation. there are other 4 modes.
            //We choose ECB(Electronic code Book)
            //padding mode(if any extra byte added)


            var cTransform = tdes.CreateEncryptor();
            //transform the specified region of bytes array to resultArray
            var resultArray =
              cTransform.TransformFinalBlock(toEncryptArray, 0,
              toEncryptArray.Length);
            //Release resources held by TripleDes Encryptor
            tdes.Clear();
            //Return the encrypted data into unreadable string format
            return Convert.ToBase64String(resultArray, 0, resultArray.Length);

        }

        /// <summary>
        /// Decrypts input encrypted string using TripleDES algorithm as implemented by .Net's CryptoServiceProvider
        /// input string is converted in byteArray and then decrypted byte by byte
        /// </summary>
        /// <param name="cypherText">input string as encrypted</param>
        /// <param name="key"></param>
        /// <returns>plain text as result of decryption</returns>
        public static string Decrypt(string cypherText, string key)
        {

            byte[] keyArray;
            //get the byte code of the string

            byte[] toEncryptArray = Convert.FromBase64String(cypherText);

            //System.Configuration.AppSettingsReader settingsReader =
            //                                    new AppSettingsReader();
            //Get your key from config file to open the lock!
            //string key = (string)settingsReader.GetValue("SecurityKey",
            //                                             typeof(String));

            if (true)
            {
                //if hashing was used get the hash code with regards to your key
                MD5CryptoServiceProvider hashmd5 = new MD5CryptoServiceProvider();
                keyArray = hashmd5.ComputeHash(Encoding.UTF8.GetBytes(key));
                //release any resource held by the MD5CryptoServiceProvider

                hashmd5.Clear();
            }
            //else
            //{
            //    //if hashing was not implemented get the byte code of the key
            //    keyArray = UTF8Encoding.UTF8.GetBytes(key);
            //}

            var tdes = new TripleDESCryptoServiceProvider
            {
                Key = keyArray,
                Mode = CipherMode.ECB,
                Padding = PaddingMode.PKCS7
            };
            //set the secret key for the tripleDES algorithm
            //mode of operation. there are other 4 modes. 
            //We choose ECB(Electronic code Book)

            //padding mode(if any extra byte added)

            var cTransform = tdes.CreateDecryptor();
            var resultArray = cTransform.TransformFinalBlock(
                                 toEncryptArray, 0, toEncryptArray.Length);
            //Release resources held by TripleDes Encryptor                
            tdes.Clear();
            //return the Clear decrypted TEXT
            return Encoding.UTF8.GetString(resultArray);


        }
/*
        public static string GetRandomKey(int length, string allowedChars = "abcdefghijklmnopqrstuvABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789")
        {
            if (length < 0) throw new ArgumentOutOfRangeException("length", "length cannot be less than zero.");
            if (string.IsNullOrEmpty(allowedChars)) throw new ArgumentException("allowedChars may not be empty.");

            const int byteSize = 0x100;
            var allowedCharSet = new HashSet<char>(allowedChars).ToArray();
            if (byteSize < allowedCharSet.Length) throw new ArgumentException(String.Format("allowedChars may contain no more than {0} characters.", byteSize));

            // Guid.NewGuid and System.Random are not particularly random. By using a
            // cryptographically-secure random number generator, the caller is always
            // protected, regardless of use.
            using (var rng = new RNGCryptoServiceProvider())
            {
                var result = new StringBuilder();
                var buf = new byte[128];
                while (result.Length < length)
                {
                    rng.GetBytes(buf);
                    for (var i = 0; i < buf.Length && result.Length < length; ++i)
                    {
                        // Divide the byte into allowedCharSet-sized groups. If the
                        // random value falls into the last group and the last group is
                        // too small to choose from the entire allowedCharSet, ignore
                        // the value in order to avoid biasing the result.
                        var outOfRangeStart = byteSize - (byteSize % allowedCharSet.Length);
                        if (outOfRangeStart <= buf[i]) continue;
                        result.Append(allowedCharSet[buf[i] % allowedCharSet.Length]);
                    }
                }
                return result.ToString();
            }
        }
*/

/*
        public static byte[] Decrypt(byte[] cypherText)
        {

            byte[] keyArray;

            //get the byte code of the string
            byte[] toEncryptArray = cypherText; //Convert.FromBase64String(cypherText);


            keyArray = UTF8Encoding.UTF8.GetBytes(PRIVATE_KEY);

            TripleDESCryptoServiceProvider tdes = new TripleDESCryptoServiceProvider();
            //set the secret key for the tripleDES algorithm
            tdes.Key = keyArray;
            //mode of operation. there are other 4 modes.
            //We choose ECB(Electronic code Book)

            tdes.Mode = CipherMode.ECB;
            //padding mode(if any extra byte added)
            tdes.Padding = PaddingMode.PKCS7;

            ICryptoTransform cTransform = tdes.CreateDecryptor();
            byte[] resultArray = cTransform.TransformFinalBlock
                    (toEncryptArray, 0, toEncryptArray.Length);
            //Release resources held by TripleDes Encryptor
            tdes.Clear();
            //return the Clear decrypted TEXT
            // return UTF8Encoding.UTF8.GetString(resultArray);
            return resultArray;

        }
*/

        public static byte[] Encrypt(byte[] plainText, string key)
        {

            byte[] keyArray;
            var toEncryptArray = plainText;// UTF8Encoding.UTF8.GetBytes(plainText);

           // var appSettingsReader = new AppSettingsReader();
            // Get the key from config file

            //string key = (string)settingsReader.GetValue(key,
            //                                                 typeof(String));
            //System.Windows.Forms.MessageBox.Show(key);
            //If hashing use get hashcode regards to your key
            if (true)
            {
                var hashmd5 = new MD5CryptoServiceProvider();
                keyArray = hashmd5.ComputeHash(Encoding.UTF8.GetBytes(key));
                //Always release the resources and flush data
                // of the Cryptographic service provide. Best Practice

                hashmd5.Clear();
            }
            //else
            //   keyArray = UTF8Encoding.UTF8.GetBytes(key);

            var tdes = new TripleDESCryptoServiceProvider
            {
                Key = keyArray,
                Mode = CipherMode.ECB,
                Padding = PaddingMode.PKCS7
            };
            //set the secret key for the tripleDES algorithm
            //mode of operation. there are other 4 modes.
            //We choose ECB(Electronic code Book)
            //padding mode(if any extra byte added)


            var cTransform = tdes.CreateEncryptor();
            //transform the specified region of bytes array to resultArray
            var resultArray =
              cTransform.TransformFinalBlock(toEncryptArray, 0,
              toEncryptArray.Length);
            //Release resources held by TripleDes Encryptor
            tdes.Clear();
            //Return the encrypted data into unreadable string format
            // return Convert.ToBase64String(resultArray, 0, resultArray.Length);

            return resultArray;
        }

        public static byte[] ObjectToByteArray(object obj)
        {
            if (obj == null)
                return null;
            var bf = new BinaryFormatter();
            using (var ms = new MemoryStream())
            {
                bf.Serialize(ms, obj);
                return ms.ToArray();
            }
        }

        public static object ByteArrayToObject(byte[] arrBytes)
        {
            using (var memStream = new MemoryStream())
            {
                var binForm = new BinaryFormatter();
                memStream.Write(arrBytes, 0, arrBytes.Length);
                memStream.Seek(0, SeekOrigin.Begin);
                var obj = binForm.Deserialize(memStream);
                return obj;
            }
           
        }
        public static byte[] Decrypt(byte[] cypherText, string key)
        {

            byte[] keyArray;
            //get the byte code of the string

            var toEncryptArray = cypherText; //Convert.FromBase64String(cypherText);

            //new AppSettingsReader();
            //Get your key from config file to open the lock!
            //string key = (string)settingsReader.GetValue("SecurityKey",
            //                                             typeof(String));

            if (true)
            {
                //if hashing was used get the hash code with regards to your key
                MD5CryptoServiceProvider hashmd5 = new MD5CryptoServiceProvider();
                keyArray = hashmd5.ComputeHash(Encoding.UTF8.GetBytes(key));
                //release any resource held by the MD5CryptoServiceProvider

                hashmd5.Clear();
            }
            //else
            //{
            //    //if hashing was not implemented get the byte code of the key
            //    keyArray = UTF8Encoding.UTF8.GetBytes(key);
            //}

            var tdes = new TripleDESCryptoServiceProvider
            {
                Key = keyArray,
                Mode = CipherMode.ECB,
                Padding = PaddingMode.PKCS7
            };
            //set the secret key for the tripleDES algorithm
            //mode of operation. there are other 4 modes. 
            //We choose ECB(Electronic code Book)

            //padding mode(if any extra byte added)

            var cTransform = tdes.CreateDecryptor();
            var resultArray = cTransform.TransformFinalBlock(
                                 toEncryptArray, 0, toEncryptArray.Length);
            //Release resources held by TripleDes Encryptor                
            tdes.Clear();
            //return the Clear decrypted TEXT
            //  return UTF8Encoding.UTF8.GetString(resultArray);
            return resultArray;

        }
    }
}
