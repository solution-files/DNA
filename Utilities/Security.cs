#region Usings

using IdentityModel;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;

#endregion

namespace Utilities {

    public class Security {

        #region Passwords

        // Valid Password Rules
        public static string ValidPasswordRules(PasswordOptions opts = null) {
            StringBuilder result = new StringBuilder();
            try {
                if (opts == null) opts = new PasswordOptions() {
                    RequiredLength = Conf.AppSetting<int>("PasswordRules:RequiredLength"),
                    RequiredUniqueChars = Conf.AppSetting<int>("PasswordRules:RequiredUniqueChars"),
                    RequireDigit = Conf.AppSetting<bool>("PasswordRules:RequiedDigit"),
                    RequireLowercase = Conf.AppSetting<bool>("PasswordRules:RequireLowerCase"),
                    RequireNonAlphanumeric = Conf.AppSetting<bool>("PasswordRules:RequireNonAlphanumeric"),
                    RequireUppercase = Conf.AppSetting<bool>("PasswordRules:RequireUppercase")
                };

                result.AppendLine("<h6>Passwords must:</h6>");
                result.AppendLine("<ul class='small'>");
                result.AppendLine($"<li>Consist of at least {opts.RequiredLength} characters</li>");
                if (opts.RequireUppercase) {
                    result.AppendLine($"<li>Include an upper case character</li>");
                }
                if (opts.RequireLowercase) {
                    result.AppendLine($"<li>Include a lower case character</li>");
                }
                if (opts.RequireDigit) {
                    result.AppendLine($"<li>Include a numeric value</li>");
                }
                if (opts.RequireNonAlphanumeric) {
                    result.AppendLine($"<li>Include a non-alphanumeric character</li>");
                }
                if (opts.RequiredUniqueChars > 0) {
                    result.AppendLine($"<li>Consist of at least {opts.RequiredUniqueChars} unique characters</li>");
                }
                result.AppendLine("</ul>");

            } catch (Exception ex) {
                result.AppendLine(ex.Message);
            }
            return result.ToString();
        }

        // Valid Password Options
        public static string ValidPasswordOptions(string value, PasswordOptions opts = null) {
            string result = "";
            try {
                if (opts == null) opts = new PasswordOptions() {
                    RequiredLength = 8,
                    RequiredUniqueChars = 6,
                    RequireDigit = true,
                    RequireLowercase = true,
                    RequireNonAlphanumeric = true,
                    RequireUppercase = true
                };

                if (value.Length < opts.RequiredLength) {
                    result = $"Must be at least {opts.RequiredLength} characters";
                } else if (opts.RequireUppercase) {
                    if (value.Count(char.IsUpper) < 1) {
                        result = "Must include an upper case character";
                    } else if (opts.RequireLowercase) {
                        if (value.Count(char.IsLower) < 1) {
                            result = "Must include a lower case character";
                        } else if (opts.RequireDigit) {
                            if (value.Count(char.IsNumber) < 1) {
                                result = "Must include a numeric value";
                            } else if (opts.RequireNonAlphanumeric) {
                                if (value.Count(char.IsPunctuation) + value.Count(char.IsSymbol) < 1) {
                                    result = "Must include a non-alphanumeric character";
                                } else if (value.Distinct().Count() < opts.RequiredUniqueChars) {
                                    result = $"{opts.RequiredUniqueChars} of {opts.RequiredLength} characters must be unique";
                                }
                            }
                        }
                    }
                }
            } catch (Exception ex) {
                result = ex.Message;
            }
            return result;
        }

        // Generate Random Password
        public static string GenerateRandomPassword(PasswordOptions opts = null) {
            if (opts == null) opts = new PasswordOptions() {
                RequiredLength = 8,
                RequiredUniqueChars = 3,
                RequireDigit = true,
                RequireLowercase = true,
                RequireNonAlphanumeric = true,
                RequireUppercase = true
            };

            string[] randomChars = new[] {
                "ABCDEFGHJKLMNOPQRSTUVWXYZ",    // uppercase 
				"abcdefghijkmnopqrstuvwxyz",    // lowercase
				"0123456789",                   // digits
				"~!@#$%*_-"                        // non-alphanumeric
			};

            CryptoRandom rand = new CryptoRandom();
            List<char> chars = new List<char>();

            if (opts.RequireUppercase) {
                chars.Insert(rand.Next(0, chars.Count), randomChars[0][rand.Next(0, randomChars[0].Length)]);
            }

            if (opts.RequireLowercase) {
                chars.Insert(rand.Next(0, chars.Count), randomChars[1][rand.Next(0, randomChars[1].Length)]);
            }

            if (opts.RequireDigit) {
                chars.Insert(rand.Next(0, chars.Count), randomChars[2][rand.Next(0, randomChars[2].Length)]);
            }

            if (opts.RequireNonAlphanumeric) {
                chars.Insert(rand.Next(0, chars.Count), randomChars[3][rand.Next(0, randomChars[3].Length)]);
            }

            for (int i = chars.Count; i < opts.RequiredLength || chars.Distinct().Count() < opts.RequiredUniqueChars; i++) {
                string rcs = randomChars[rand.Next(0, randomChars.Length)];
                chars.Insert(rand.Next(0, chars.Count),
                    rcs[rand.Next(0, rcs.Length)]);
            }

            return new string(chars.ToArray());
        }

        // Create Hash
        public static string CreateHash(string value) {
            int SaltByteSize = 24;
            int HashByteSize = 20;
            int Pbkdf2Iterations = 310000;
            string result;
            try {
                RNGCryptoServiceProvider cryptoProvider = new RNGCryptoServiceProvider();
                byte[] salt = new byte[SaltByteSize];
                cryptoProvider.GetBytes(salt);
                byte[] hash = GetPbkdf2Bytes(value, salt, Pbkdf2Iterations, HashByteSize);
                result = Pbkdf2Iterations + ":" + Convert.ToBase64String(salt) + ":" + Convert.ToBase64String(hash);
            } catch {
                result = value;
            }
            return result;
        }

        // Validate Password
        public static bool ValidatePassword(string password, string correctHash) {
            int IterationIndex = 0;
            int SaltIndex = 1;
            int Pbkdf2Index = 2;
            bool result;
            try {
                char[] delimiter = { ':' };
                string[] split = correctHash.Split(delimiter);
                int iterations = Int32.Parse(split[IterationIndex]);
                byte[] salt = Convert.FromBase64String(split[SaltIndex]);
                byte[] hash = Convert.FromBase64String(split[Pbkdf2Index]);
                byte[] testHash = GetPbkdf2Bytes(password, salt, iterations, hash.Length);
                result = SlowEquals(hash, testHash);
            } catch {
                result = false;
            }
            return result;
        }

        // Get PBKDF2 Bytes
        private static byte[] GetPbkdf2Bytes(string password, byte[] salt, int iterations, int outputBytes) {
            Rfc2898DeriveBytes pbkdf2 = new Rfc2898DeriveBytes(password, salt) {
                IterationCount = iterations
            };
            return pbkdf2.GetBytes(outputBytes);
        }

        // Slow Equals
        private static bool SlowEquals(byte[] a, byte[] b) {
            uint diff = (uint)a.Length ^ (uint)b.Length;
            for (int i = 0; i < a.Length && i < b.Length; i++) {
                diff |= (uint)(a[i] ^ b[i]);
            }
            return diff == 0;
        }

        #endregion

        #region Certificates

        // Create Self-Signed Certificate
        public static void CreateSelfSignedCertificate(string name) {
            using (RSA rsa = RSA.Create()) {
                CertificateRequest request = new(
                    $"CN={name}",
                    rsa,
                    HashAlgorithmName.SHA256,
                    RSASignaturePadding.Pkcs1);
                request.CertificateExtensions.Add(new X509BasicConstraintsExtension(
                    false, false, 0, true));
                request.CertificateExtensions.Add(new X509EnhancedKeyUsageExtension(
                    new OidCollection { new Oid("1.3.6.1.5.5.7.3.1") }, true));
                DateTimeOffset notBefore = DateTimeOffset.UtcNow;
                DateTimeOffset notAfter = notBefore.AddYears(1);
                X509Certificate2 certificate = request.CreateSelfSigned(notBefore, notAfter);
                string certFilePath = $"{name}.pfx";
                string certPassword = ""; // Set a password to protect the private key
                File.WriteAllBytes(certFilePath, certificate.Export(X509ContentType.Pfx, certPassword));
            }
        }

        // Get Certificate From Store
        private static X509Certificate2 GetCertificateFromStore(string certName) {

            // Get the certificate store for the current user.
            X509Store store = new X509Store(StoreLocation.CurrentUser);
            try {
                store.Open(OpenFlags.ReadOnly);
                X509Certificate2Collection certCollection = store.Certificates;
                // If using a certificate with a trusted root you do not need to FindByTimeValid, instead:
                // currentCerts.Find(X509FindType.FindBySubjectDistinguishedName, certName, true);
                X509Certificate2Collection currentCerts = certCollection.Find(X509FindType.FindByTimeValid, DateTime.Now, false);
                X509Certificate2Collection signingCert = currentCerts.Find(X509FindType.FindBySubjectDistinguishedName, certName, false);
                if (signingCert.Count == 0)
                    return null;
                // Return the first certificate in the collection, has the right name and is current.
                return signingCert[0];
            } finally {
                store.Close();
            }
        }

        #endregion

    }

}
