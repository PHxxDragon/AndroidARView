using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using ZXing;

namespace EAR.QRCode
{
    public class QRCodeReader : MonoBehaviour
    {
        private WebCamTexture webCamTexture;
        private IBarcodeReader barcodeReader;
        private bool camAvailable;

        public AspectRatioFitter fit;
        public RawImage background;

        public event Action<string> QRCodeRecognizedEvent;
        void Start()
        {
            WebCamDevice[] devices = WebCamTexture.devices;
            if (devices.Length == 0)
            {
                Debug.Log("No camera");
                camAvailable = false;
                return;
            }

            for (int i = 0; i < devices.Length; i++)
            {
                if (!devices[i].isFrontFacing)
                {
                    webCamTexture = new WebCamTexture(devices[i].name, Screen.width, Screen.height);
                }
            }

            if (webCamTexture == null)
            {
                webCamTexture = new WebCamTexture(devices[0].name, Screen.width, Screen.height);
                Debug.Log("No back camera found, using front camera instead.");
            }
            barcodeReader = new BarcodeReader();
            barcodeReader.Options.PossibleFormats = new List<BarcodeFormat>();
            barcodeReader.Options.PossibleFormats.Add(BarcodeFormat.QR_CODE);
            barcodeReader.Options.TryHarder = false;
            background.texture = webCamTexture;
            camAvailable = true;
            StartCoroutine(PlayWebCam());
            StartCoroutine(CheckQRCode());
        }

        private IEnumerator PlayWebCam()
        {
            webCamTexture.Play();
            if (!webCamTexture.isPlaying)
            {
                yield return new WaitForSeconds(0.2f);
                yield return StartCoroutine(PlayWebCam());
            }
        }

        void Update()
        {
            if (camAvailable && webCamTexture.isPlaying)
            {
                float ratio = (float)webCamTexture.width / webCamTexture.height;
                fit.aspectRatio = ratio;

                float scaleY = webCamTexture.videoVerticallyMirrored ? -1f : 1f;
                background.rectTransform.localScale = new Vector3(1, scaleY, 1);

                int orient = -webCamTexture.videoRotationAngle;
                background.rectTransform.localEulerAngles = new Vector3(0, 0, orient);
            }
        }

        public void StopScan()
        {
            webCamTexture.Stop();
        }

        private IEnumerator CheckQRCode()
        {
            while (true)
            {
                if (camAvailable)
                {
                    if (webCamTexture.isPlaying)
                    {
                        Result result = barcodeReader.Decode(webCamTexture.GetPixels32(), webCamTexture.width, webCamTexture.height);
                        if (result != null)
                        {
                            QRCodeRecognizedEvent?.Invoke(result.Text);
                            yield return new WaitForSeconds(3);
                        }
                        else
                        {
                            yield return new WaitForSeconds(3);
                        }
                    } else
                    {
                        yield return new WaitForSeconds(1);
                    }
                    
                } else {
                    break;
                }
            }
        }
    }
}

