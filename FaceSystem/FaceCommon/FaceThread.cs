using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FaceSystem.FaceCommon
{
  public  class FaceThread
    {

        public FaceThread(MainWindow mainFrom)
        {
            try
            {
                _mainForm = mainFrom;
                _stop = false;
                faces = new QSNetFaceEngine.FaceModel[1];
                faces2 = new QSNetFaceEngine.FaceModel[2];
                _haveImage = new AutoResetEvent(false);
                //初始化人脸引擎
                qsFaceEngine = new QSNetFaceEngine();
                bool tag = qsFaceEngine.initial();
                if (!tag)
                {
                    return;
                }


            }
            catch (Exception ex)
            {


            }

        }

        ~FaceThread()
        {

        }

        public void Start(Image<Bgr, byte> image)
        {
            try
            {
                _frameImage = new Image<Bgr, byte>(image.Bitmap);
                _haveImage.Set();
            }
            catch (Exception)
            {
            }

        }

        public bool GetResults(QSNetFaceEngine.RECT rtFace)
        {
            if (_working)
            {
                return false;
            }
            if (contain_face)
            {
                rtFace = faces[0].rtFace;
                return true;
            }
            return false;
        }
        public void Execute()
        {
            //while (!_stop)
            //{

            //    _working = false;
            //    _haveImage.WaitOne();
            //    _working = true;
            //    if (!_stop)
            //    {
            //        _faceCount = qsFaceEngine.detectFaces(_frameImage.Bytes, _frameImage.Cols, _frameImage.Rows, _frameImage.MIplImage.widthStep, faces, 1);
            //        Console.WriteLine("find face_num=" + _faceCount);
            //        if (_faceCount > 0)
            //        {
            //            contain_face = true;
            //            if (_mainForm != null && _mainForm.idCardImagePath != "")
            //            {
            //                Compare2face();
            //            }
            //        }
            //        else
            //        {
            //            contain_face = false;
            //            _faceCount = 0;
            //        }

            //    }
            //}
        }

        //private void Compare2face()
        //{
        //    float score = qsFaceEngine.compare2Feature(faces[0].feature, faces2[0].feature);
        //    Console.WriteLine(score);
        //    if (_mainForm.GetFaceThreshold() <= score)
        //    {
        //        _mainForm.StopTimer();
        //        _mainForm.SetSimilarity(score);

        //        _mainForm.ShowMsg("faceSuccess");
        //        _lastScore = 0;
        //    }
        //    else 
        //    {
        //        //_mainForm.SetSimilarity(score);
        //    }
        //}

        public void Stop()
        {
            _stop = true;
            Console.WriteLine("FaceThread stop.");
        }

        public void Start()
        {
            _stop = false;
        }

        public MainWindow _mainForm { get; set; }
        public bool _stop { get; set; }
        public Image<Bgr, byte> _frameImage { get; set; }
        public AutoResetEvent _haveImage { get; set; }

        public Bitmap _faceImage { get; set; }

        public bool _working { get; set; }

        public int _faceCount { get; set; }

        public QSNetFaceEngine.FaceModel[] faces { get; set; }
        public QSNetFaceEngine.FaceModel[] faces2 { get; set; }

        public QSNetFaceEngine qsFaceEngine { get; set; }

        public bool contain_face { get; set; }

        public int DecfNum { get; set; }
        public int contain_num { get; set; }

        public int _count { get; set; }

        public IntPtr processor { get; set; }

        public HaarCascade haar { get; set; }

        public MCvAvgComp[][] facesDetected { get; set; }

        private float _lastScore = 0f;

    }
}
