using UnityEngine;
using UnityEngine.Networking;
using System;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Localization.Settings;

namespace EAR.WebRequest
{
    public class WebRequestHelper : MonoBehaviour
    {
        private const string TOKEN_KEY = "token";
        private const string EMAIL_KEY = "email";
        private const string PASSWORD_KEY = "password";
        private const string WRONG_CREDENTIALS = "WrongCredentials";
        private const string UNEXPECTED = "UnexpectedConnectionError";
        private const string NO_CONNECTION = "NoConnection";
        private const string TOKEN_EXPIRED_ERROR = "TokenExpiredError";

        private ApplicationConfiguration applicationConfiguration;
        private Dictionary<string, Dictionary<int, string>> localizedCategory = new Dictionary<string, Dictionary<int, string>>();
        private string[] locales = { "en", "vi" };

        void Start()
        {
            StartCoroutine(FetchCategory());
        }

        public string GetLocalizedCategory(int categoryId)
        {
            try
            {
                string locale = LocalizationSettings.Instance.GetSelectedLocale().Identifier.Code;
                return localizedCategory[locale][categoryId];
            } catch (Exception e)
            {
                Debug.Log(e);
                return categoryId.ToString();
            }
        }

        private IEnumerator FetchCategory()
        {
            while (true)
            {
                bool failed = false;
                foreach (string langCode in locales)
                {
                    if (!localizedCategory.ContainsKey(langCode))
                    {
                        GetCategory(langCode, 
                        (response) =>
                        {
                            Dictionary<int, string> categoryDict = new Dictionary<int, string>();
                            foreach (CategoryDataObject categoryData in response)
                            {
                                categoryDict.Add(categoryData.id, categoryData.name);
                            }
                            localizedCategory.Add(langCode, categoryDict);
                        }, 
                        (error) =>
                        {
                            failed = true;
                        });
                    }
                }
                yield return new WaitForSecondsRealtime(3f);
                if (!failed)
                {
                    break;
                }
            }
        }
 
        public void GetCategory(string langCode, Action<List<CategoryDataObject>> callback = null, Action<string> errorCallback = null)
        {
            string url = applicationConfiguration.GetCategoryPath(langCode);
            StartCoroutine(GetRequestCoroutine<CategoryDataResponse>(url,
                (response) =>
                {
                    callback?.Invoke(response.data);
                }, (error, errorCode) => {
                    errorCallback?.Invoke(error);
                }));
        }

        public void Logout()
        {
            LocalStorage.Save(TOKEN_KEY, "");
            LocalStorage.Save(EMAIL_KEY, "");
            LocalStorage.Save(PASSWORD_KEY, "");
        }

        public void Login(Action<UserProfileData> callback = null, Action<string> errorCallback = null)
        {
            Login(LocalStorage.Load(EMAIL_KEY), LocalStorage.Load(PASSWORD_KEY), callback, errorCallback);
        }

        public void Login(string email, string password, Action<UserProfileData> callback = null, Action<string> errorCallback = null)
        {
            LoginRequest loginData = new LoginRequest();
            loginData.email = email;
            loginData.password = password;

            StartCoroutine(PostRequestCoroutine<LoginRequest, LoginResponse>(applicationConfiguration.GetLoginPath(), loginData,
                (response) =>
                {
                    LocalStorage.Save(TOKEN_KEY, response.token);
                    LocalStorage.Save(EMAIL_KEY, email);
                    LocalStorage.Save(PASSWORD_KEY, password);
                    callback?.Invoke(response.data.user);
                }, 
                (error, errorCode) =>
                {
                    if (errorCode == 400)
                    {
                        errorCallback?.Invoke(LocalizationUtils.GetLocalizedText(WRONG_CREDENTIALS));
                    } else if (errorCode == -1)
                    {
                        errorCallback?.Invoke(error);
                    } else
                    {
                        errorCallback?.Invoke(LocalizationUtils.GetLocalizedText(UNEXPECTED));
                    }
                }));
        }

        public void GetARModuleData(int id, Action<AssetInformation> callback = null, Action<string> errorCallback = null)
        {
            string url = applicationConfiguration.GetARModulePath(id);
            StartCoroutine(GetRequestCoroutine<AssetInformationResponse>(url,
                (response) =>
                {
                    callback?.Invoke(response.data);
                }, 
                (error, errorCode) => {
                    if (errorCode == -1)
                    {
                        errorCallback?.Invoke(error);
                    }
                    else
                    {
                        errorCallback?.Invoke(LocalizationUtils.GetLocalizedText(UNEXPECTED));
                    }
                }));
        }

        public void GetModelARData(int id, Action<AssetInformation> callback = null, Action<string> errorCallback = null)
        {
            string url = applicationConfiguration.GetModelARDataPath(id);
            StartCoroutine(GetRequestCoroutine<AssetInformationResponse>(url,
                (response) =>
                {
                    callback?.Invoke(response.data);
                },
                (error, errorCode) =>
                {
                    if (errorCode == -1)
                    {
                        errorCallback?.Invoke(error);
                    }
                    else
                    {
                        errorCallback?.Invoke(LocalizationUtils.GetLocalizedText(UNEXPECTED));
                    }
                }));
        }

        public void GetModuleList(int courseId, Action<List<SectionData>> callback = null, Action<string> errorCallback = null)
        {
            string url = applicationConfiguration.GetModuleListPath(courseId);
            StartCoroutine(GetRequestCoroutine<ModuleDataResponse>(url,
            (response) =>
            {
                callback?.Invoke(response.data.sections);
            }, 
            (error, errorCode) => {
                if (errorCode == -1)
                {
                    errorCallback?.Invoke(error);
                }
                else
                {
                    errorCallback?.Invoke(LocalizationUtils.GetLocalizedText(UNEXPECTED));
                }
            }));
        }

        public void GetCourseList(int page, int limit, string type, string keyword, Action<CourseListData> callback = null, Action<string> errorCallback = null)
        {
            string url = applicationConfiguration.GetCourseListPath(page, limit, type, keyword);
            StartCoroutine(GetRequestCoroutine<CourseListDataResponse>(url,
                (response) =>
                {
                    callback?.Invoke(response.data);
                }, 
                (error, errorCode) => {
                    if (errorCode == -1)
                    {
                        errorCallback?.Invoke(error);
                    } else
                    {
                        errorCallback?.Invoke(LocalizationUtils.GetLocalizedText(UNEXPECTED));
                    }
                }));
        }

        public void GetModelDetail(int id, Action<ModelDataObject> callback = null, Action<string> errorCallback = null)
        {
            string url = applicationConfiguration.GetModelPath(id);
            StartCoroutine(GetRequestCoroutine<ModelDetailResponse>(url,
                (response) =>
                {
                    callback?.Invoke(response.data);
                }, (error, errorCode) =>
                {
                    if (errorCode == -1)
                    {
                        errorCallback?.Invoke(error);
                    }
                    else
                    {
                        errorCallback?.Invoke(LocalizationUtils.GetLocalizedText(UNEXPECTED));
                    }
                }));
        }

        public void GetModelList(int page, int limit, string filter, string keyword, Action<ModelListResponseData> callback = null, Action<string> errorCallback = null)
        {
            string url = applicationConfiguration.GetModelListPath(page, limit, filter, keyword);
            StartCoroutine(GetRequestCoroutine<ModelListResponse>(url,
                (response) =>
                {
                    callback?.Invoke(response.data);
                },
                (error, errorCode) =>
                {
                    if (errorCode == -1)
                    {
                        errorCallback?.Invoke(error);
                    } else
                    {
                        errorCallback?.Invoke(LocalizationUtils.GetLocalizedText(UNEXPECTED));
                    }
                }));
        }

        public void GetInfoFromQRCode(string qrToken, Action<AssetInformation> callback = null, Action<string> errorCallback = null)
        {
            string url = applicationConfiguration.GetQRCodePath(qrToken);
            StartCoroutine(GetRequestCoroutine<AssetInformationResponse>(url,
                (response) =>
                {
                    callback?.Invoke(response.data);
                },
                (error, errorCode) =>
                {
                    if (errorCode == -1)
                    {
                        errorCallback?.Invoke(error);
                    }
                    else
                    {
                        errorCallback?.Invoke(LocalizationUtils.GetLocalizedText(UNEXPECTED));
                    }
                }
                ));
        }

        void Awake()
        {
            applicationConfiguration = ApplicationConfigurationHolder.Instance.GetApplicationConfiguration();
        }

        public IEnumerator GetRequestCoroutine<T>(string url, Action<T> callback, Action<string, long> errorCallback, bool retried = false)
        {
            Debug.Log("url: " + url);
            string token = LocalStorage.Load(TOKEN_KEY);
            using UnityWebRequest unityWebRequest = UnityWebRequest.Get(url);
            unityWebRequest.SetRequestHeader("Authorization", "Bearer " + token);
            yield return unityWebRequest.SendWebRequest();
            if (unityWebRequest.result != UnityWebRequest.Result.Success)
            {
                if (unityWebRequest.result == UnityWebRequest.Result.ConnectionError)
                {
                    Debug.Log("No connection");
                    errorCallback?.Invoke(LocalizationUtils.GetLocalizedText(NO_CONNECTION), -1);
                } else
                {
                    //ErrorResponse errorResponse = JsonUtility.FromJson<ErrorResponse>(unityWebRequest.downloadHandler.text);
                    if (!retried)
                    {
                        Debug.Log("Token expired!");
                        Login((response) =>
                        {
                            StartCoroutine(GetRequestCoroutine(url, callback, errorCallback, true));
                        }, (error) => {
                            errorCallback?.Invoke(unityWebRequest.error, unityWebRequest.responseCode);
                        });
                    } else
                    {
                        Debug.Log("error: " + unityWebRequest.downloadHandler.text);
                        Debug.Log("Error Url: " + unityWebRequest.url);
                        errorCallback?.Invoke(unityWebRequest.error, unityWebRequest.responseCode);
                    }
                }
            } else
            {
                string requestResult = unityWebRequest.downloadHandler.text;
                Debug.Log(unityWebRequest.downloadHandler.text);
                T response = JsonUtility.FromJson<T>(requestResult);
                callback?.Invoke(response);
            }
        }

        public IEnumerator PostRequestCoroutine<T1, T2>(string url, T1 requestData, Action<T2> callback, Action<string, long> errorCallback)
        {
/*            Debug.Log("url: " + url);*/
            string requestBody = JsonUtility.ToJson(requestData);
            using UnityWebRequest unityWebRequest = new UnityWebRequest(url, "POST");
            unityWebRequest.SetRequestHeader("Content-Type", "application/json");
            unityWebRequest.uploadHandler = new UploadHandlerRaw(Encoding.UTF8.GetBytes(requestBody));
            unityWebRequest.downloadHandler = new DownloadHandlerBuffer();
            yield return unityWebRequest.SendWebRequest();
            if (unityWebRequest.result != UnityWebRequest.Result.Success)
            {
                if (unityWebRequest.result == UnityWebRequest.Result.ConnectionError)
                {
                    errorCallback?.Invoke(LocalizationUtils.GetLocalizedText(NO_CONNECTION), -1);
                } else
                {
                    errorCallback?.Invoke(unityWebRequest.error, unityWebRequest.responseCode);
                }
            }
            else
            {
                string requestResult = unityWebRequest.downloadHandler.text;
                T2 response = JsonUtility.FromJson<T2>(requestResult);
                callback?.Invoke(response);
            }
        }

        public string GetAuthorizeToken()
        {
            return LocalStorage.Load(TOKEN_KEY);
        }
    }
}

