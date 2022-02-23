using UnityEngine;
using UnityEngine.Networking;
using System;
using System.Text;
using System.Collections;
using System.Collections.Generic;

namespace EAR.WebRequest
{
    public class WebRequestHelper : MonoBehaviour
    {
        private const string TOKEN_KEY = "token";
        private const string WRONG_CREDENTIALS = "Wrong username or password!";
        private const string UNEXPECTED = "Unexpected error happened!";

        private ApplicationConfiguration applicationConfiguration;

        public void Login(string email, string password, Action callback = null, Action<string> errorCallback = null)
        {
            StartCoroutine(LoginCoroutine(email, password, callback, errorCallback));
        }

        public void GetProfile(string token, Action<UserProfileData> callback, Action<string> errorCallback = null)
        {
            StartCoroutine(GetProfileCoroutine(token, callback, errorCallback));
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

        private IEnumerator GetProfileCoroutine(string token, Action<UserProfileData> callback, Action<string> errorCallback)
        {
            UserProfileRequest userProfileRequest = new UserProfileRequest();
            userProfileRequest.token = token;

            string serializedUserProfileRequest = JsonUtility.ToJson(userProfileRequest);

            using (UnityWebRequest unityWebRequest = new UnityWebRequest(applicationConfiguration.GetProfilePath(), "POST"))
            {
                unityWebRequest.SetRequestHeader("Content-Type", "application/json");
                unityWebRequest.uploadHandler = new UploadHandlerRaw(Encoding.UTF8.GetBytes(serializedUserProfileRequest));
                unityWebRequest.downloadHandler = new DownloadHandlerBuffer();
                yield return unityWebRequest.SendWebRequest();
                if (unityWebRequest.result != UnityWebRequest.Result.Success)
                {
                    Debug.Log(unityWebRequest.error);
                    errorCallback?.Invoke(UNEXPECTED);
                } else
                {
                    string requestResult = unityWebRequest.downloadHandler.text;
                    UserProfileResponse response = JsonUtility.FromJson<UserProfileResponse>(requestResult);
                    callback?.Invoke(response.data.user);
                }
            }
        }

        private IEnumerator LoginCoroutine(string email, string password, Action callback, Action<string> errorCallback)
        {
            LoginRequest loginData = new LoginRequest();
            loginData.email = email;
            loginData.password = password;

            string serializedLoginData = JsonUtility.ToJson(loginData);

            using (UnityWebRequest unityWebRequest = new UnityWebRequest(applicationConfiguration.GetLoginPath(), "POST"))
            {
                unityWebRequest.SetRequestHeader("Content-Type", "application/json");
                unityWebRequest.uploadHandler = new UploadHandlerRaw(Encoding.UTF8.GetBytes(serializedLoginData));
                unityWebRequest.downloadHandler = new DownloadHandlerBuffer();
                yield return unityWebRequest.SendWebRequest();
                if (unityWebRequest.result != UnityWebRequest.Result.Success)
                {
                    if (unityWebRequest.result == UnityWebRequest.Result.ConnectionError)
                    {
                        Debug.Log(unityWebRequest.error);
                        errorCallback?.Invoke(UNEXPECTED);
                    } else if (unityWebRequest.result == UnityWebRequest.Result.ProtocolError)
                    {
                        if (unityWebRequest.responseCode == 401)
                        {
                            errorCallback?.Invoke(WRONG_CREDENTIALS);
                        } else
                        {
                            Debug.Log(unityWebRequest.error);
                            errorCallback?.Invoke(UNEXPECTED);
                        }
                    } else
                    {
                        Debug.Log(unityWebRequest.error);
                        errorCallback?.Invoke(UNEXPECTED);
                    }
                  
                }
                else
                {
                    string requestResult = unityWebRequest.downloadHandler.text;
                    LoginResponse response = JsonUtility.FromJson<LoginResponse>(requestResult);
                    LocalStorage.Save(TOKEN_KEY, response.token);
                    callback?.Invoke();
                }
            }
        }

        public string GetAuthorizeToken()
        {
            return LocalStorage.Load(TOKEN_KEY);
        }
    }
}

