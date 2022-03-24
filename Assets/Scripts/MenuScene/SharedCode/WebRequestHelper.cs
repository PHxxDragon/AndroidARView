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
                        errorCallback?.Invoke(Utils.GetLocalizedText(WRONG_CREDENTIALS));
                    } else if (errorCode == -1)
                    {
                        errorCallback?.Invoke(error);
                    } else
                    {
                        errorCallback?.Invoke(Utils.GetLocalizedText(UNEXPECTED));
                    }
                }));
        }

        public void GetModelARData(int id, Action<ModelARDataObject> callback = null, Action<string> errorCallback = null)
        {
            string url = applicationConfiguration.GetModelARDataPath(id);
            StartCoroutine(GetRequestCoroutine<ModelARDataResponse>(url,
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
                        errorCallback?.Invoke(Utils.GetLocalizedText(UNEXPECTED));
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
                        errorCallback?.Invoke(Utils.GetLocalizedText(UNEXPECTED));
                    }
                }));
        }

        public void GetModelList(int page, int limit, bool isBoughtModel, Action<ModelListResponseData> callback = null, Action<string> errorCallback = null)
        {
            string url = isBoughtModel ? applicationConfiguration.GetBoughtModelListPath(page, limit) : applicationConfiguration.GetUploadedModelListPath(page, limit);
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
                        errorCallback?.Invoke(Utils.GetLocalizedText(UNEXPECTED));
                    }
                }));
        }

        public void GetWorkspaceList(string token, Action<List<WorkspaceData>> callback, Action<string> errorCallback = null)
        {
            StartCoroutine(GetWorkspaceListCoroutine(token, callback, errorCallback));
        }

        public void GetCourseList(string token, int workspaceid, Action<List<CourseData>> callback, Action<string> errorCallback = null)
        {
            StartCoroutine(GetCourseListCoroutine(token, workspaceid, callback, errorCallback));
        }

        public void GetModuleList(string token, int courseId, Action<List<ModuleData>> callback, Action<string> errorCallback)
        {
            StartCoroutine(GetModuleListCoroutine(token, courseId, callback, errorCallback));
        }

        public void GetInfoFromQRCode(string qrToken, Action<ModuleARInformation> callback = null, Action<string> errorCallback = null)
        {
            StartCoroutine(GetInfoFromQRCodeCoroutine(qrToken, callback, errorCallback));
        }

        private IEnumerator GetInfoFromQRCodeCoroutine(string qrToken, Action<ModuleARInformation> callback, Action<string> errorCallback)
        {
            using (UnityWebRequest unityWebRequest = UnityWebRequest.Get(applicationConfiguration.GetQRCodePath(qrToken)))
            {
                yield return unityWebRequest.SendWebRequest();
                if (unityWebRequest.result != UnityWebRequest.Result.Success)
                {
                    errorCallback?.Invoke(unityWebRequest.error);
                }
                else
                {
                    ModuleInfoResponse moduleInfoResponse = JsonUtility.FromJson<ModuleInfoResponse>(unityWebRequest.downloadHandler.text);
                    Debug.Log(unityWebRequest.downloadHandler.text);
                    callback?.Invoke(moduleInfoResponse.data);
                }
            }
        }

/*        public void GetModuleInformation(string token, int moduleId, Action<ModuleARInformation> callback = null, Action<string> errorCallback = null)
        {
            StartCoroutine(GetModuleInformationCoroutine(token, "", moduleId, callback, errorCallback));
        }

        public void GetModuleInformation(string qrToken, Action<ModuleARInformation> callback = null, Action<string> errorCallback = null)
        {
            StartCoroutine(GetModuleInformationCoroutine("", qrToken, -1, callback, errorCallback));
        }*/

/*        public void SetModuleMetadata(string token, int moduleId, MetadataObject metadata, Action callback = null, Action<string> errorCallback = null)
        {
            StartCoroutine(SetModuleMetadataCoroutine(token, moduleId, metadata, callback, errorCallback));
        }*/

        void Awake()
        {
            applicationConfiguration = ApplicationConfigurationHolder.Instance.GetApplicationConfiguration();
        }

/*        private IEnumerator SetModuleMetadataCoroutine(string token, int moduleId, MetadataObject metadata, Action callback = null, Action<string> errorCallback = null)
        {
            List<IMultipartFormSection> formData = new List<IMultipartFormSection>();
            formData.Add(new MultipartFormDataSection("metadata", JsonUtility.ToJson(metadata)));
            using (UnityWebRequest unityWebRequest = UnityWebRequest.Post(applicationConfiguration.GetARModulePath(moduleId), formData))
            {
                unityWebRequest.method = "PUT";
                unityWebRequest.SetRequestHeader("Authorization", "Bearer " + token);
                yield return unityWebRequest.SendWebRequest();
                if (unityWebRequest.result != UnityWebRequest.Result.Success)
                {
                    Debug.Log(unityWebRequest.downloadHandler.text);
                    errorCallback?.Invoke(unityWebRequest.error);
                } else
                {
                    callback?.Invoke();
                }
            }
        }*/

       /* private IEnumerator GetModuleInformationCoroutine(string token, string qrToken, int moduleId, Action<ModuleARInformation> callback, Action<string> errorCallback)
        {
            string path;
            if (moduleId != -1)
            {
                path = applicationConfiguration.GetARModulePath(moduleId);
            } else
            {
                path = applicationConfiguration.GetARModuleQRPath(qrToken);
            }
            using (UnityWebRequest unityWebRequest = UnityWebRequest.Get(path))
            {
                Debug.Log(path);
                yield return unityWebRequest.SendWebRequest();
                if (unityWebRequest.result != UnityWebRequest.Result.Success)
                {
                    errorCallback?.Invoke(unityWebRequest.error);
                }
                else
                {
                    ModuleResponse moduleResponse = JsonUtility.FromJson<ModuleResponse>(unityWebRequest.downloadHandler.text);
                    using (UnityWebRequest unityWebRequest2 = UnityWebRequest.Get(applicationConfiguration.GetModelPath(moduleResponse.data.modelId)))
                    {
                        unityWebRequest2.SetRequestHeader("Authorization", "Bearer " + token);
                        yield return unityWebRequest2.SendWebRequest();
                        if (unityWebRequest2.result != UnityWebRequest.Result.Success)
                        {
                            errorCallback?.Invoke(unityWebRequest2.error);
                        }
                        else
                        {
                            ModelDataResponse modelData = JsonUtility.FromJson<ModelDataResponse>(unityWebRequest2.downloadHandler.text);
                            ModuleARInformation moduleARInformation = new ModuleARInformation();
                            moduleARInformation.imageUrl = moduleResponse.data.image;
                            moduleARInformation.metadataString = moduleResponse.data.metadata;
                            moduleARInformation.modelUrl = modelData.data.url;
                            moduleARInformation.extension = modelData.data.extension;
                            callback?.Invoke(moduleARInformation);
                        }
                    }
                }
            }
        }*/

        /* Todo
         * 
         */
        private IEnumerator GetWorkspaceListCoroutine(string token, Action<List<WorkspaceData>> callback, Action<string> errorCallback = null)
        {
            /*using (UnityWebRequest unityWebRequest = new UnityWebRequest(applicationConfiguration.GetWorkspacePath()))
            {
                yield return unityWebRequest.SendWebRequest();
                
            }*/
            yield return null;
            List<WorkspaceData> workspaceDatas = new List<WorkspaceData>();
            WorkspaceData workspaceData = new WorkspaceData();
            workspaceData.name = "abc";
            workspaceData.owner = "Duy";
            workspaceData.role = "student";
            workspaceData.id = 1;
            workspaceData.imageUrl = "https://library.vuforia.com/sites/default/files/vuforia-library/articles/solution/Magic%20Leap%20Related%20Content/Astronaut-scaled.jpg";
            workspaceDatas.Add(workspaceData);
            workspaceData = workspaceData.Copy();
            workspaceData.id = 2;
            workspaceDatas.Add(workspaceData);
            workspaceData = workspaceData.Copy();
            workspaceData.id = 3;
            workspaceDatas.Add(workspaceData);

            callback?.Invoke(workspaceDatas);
        }

        /* TODO
         * 
         */
        private IEnumerator GetCourseListCoroutine(string token, int workspaceid, Action<List<CourseData>> callback, Action<string> errorCallback = null)
        {
            yield return null;
            List<CourseData> courseDatas = new List<CourseData>();
            CourseData course = new CourseData();
            course.id = 1;
            course.rating = 5.5f;
            course.teachers = "Duy, Khiem, Khang";
            course.title = "Science and Technology";
            course.courseType = "instructor-paced";
            course.imageUrl = "https://library.vuforia.com/sites/default/files/vuforia-library/articles/solution/Magic%20Leap%20Related%20Content/Astronaut-scaled.jpg";
            courseDatas.Add(course);
            course = new CourseData();
            course.id = 2;
            course.rating = 5.5f;
            course.teachers = "Duy, Khiem, Khang";
            course.title = "Science and Technology";
            course.courseType = "instructor-paced";
            course.imageUrl = "https://library.vuforia.com/sites/default/files/vuforia-library/articles/solution/Magic%20Leap%20Related%20Content/Astronaut-scaled.jpg";
            courseDatas.Add(course);
            course = new CourseData();
            course.id = 3;
            course.rating = 5.5f;
            course.teachers = "Duy, Khiem, Khang";
            course.title = "Science and Technology";
            course.courseType = "instructor-paced";
            course.imageUrl = "https://library.vuforia.com/sites/default/files/vuforia-library/articles/solution/Magic%20Leap%20Related%20Content/Astronaut-scaled.jpg";
            courseDatas.Add(course);
            course = new CourseData();
            course.id = 4;
            course.rating = 5.5f;
            course.teachers = "Duy, Khiem, Khang";
            course.title = "Science and Technology";
            course.courseType = "instructor-paced";
            course.imageUrl = "https://library.vuforia.com/sites/default/files/vuforia-library/articles/solution/Magic%20Leap%20Related%20Content/Astronaut-scaled.jpg";
            courseDatas.Add(course);
            callback?.Invoke(courseDatas);
        }

        private IEnumerator GetModuleListCoroutine(string token, int courseId, Action<List<ModuleData>> callback, Action<string> errorCallback)
        {
            yield return null;
            List<ModuleData> moduleDatas = new List<ModuleData>();
            ModuleData module = new ModuleData();
            module.name = "module 1";
            module.id = 1;
            moduleDatas.Add(module);
            module = new ModuleData();
            module.name = "module 2";
            module.id = 2;
            moduleDatas.Add(module);
            callback?.Invoke(moduleDatas);
        }

        public IEnumerator GetRequestCoroutine<T>(string url, Action<T> callback, Action<string, long> errorCallback, bool retried = false)
        {
            string token = LocalStorage.Load(TOKEN_KEY);
            using UnityWebRequest unityWebRequest = UnityWebRequest.Get(url);
            unityWebRequest.SetRequestHeader("Authorization", "Bearer " + token);
            yield return unityWebRequest.SendWebRequest();
            if (unityWebRequest.result != UnityWebRequest.Result.Success)
            {
                if (unityWebRequest.result == UnityWebRequest.Result.ConnectionError)
                {
                    errorCallback?.Invoke(Utils.GetLocalizedText(NO_CONNECTION), -1);
                } else
                {
                    if (unityWebRequest.responseCode == 401 && !retried)
                    {
                        Login((response) =>
                        {
                            StartCoroutine(GetRequestCoroutine<T>(url, callback, errorCallback, true));
                        }, (error) => {
                            errorCallback?.Invoke(unityWebRequest.error, unityWebRequest.responseCode);
                        });
                    } else
                    {
                        errorCallback?.Invoke(unityWebRequest.error, unityWebRequest.responseCode);
                    }
                }
            } else
            {
                string requestResult = unityWebRequest.downloadHandler.text;
                T response = JsonUtility.FromJson<T>(requestResult);
                callback?.Invoke(response);
            }
        }

        public IEnumerator PostRequestCoroutine<T1, T2>(string url, T1 requestData, Action<T2> callback, Action<string, long> errorCallback)
        {
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
                    errorCallback?.Invoke(Utils.GetLocalizedText(NO_CONNECTION), -1);
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

