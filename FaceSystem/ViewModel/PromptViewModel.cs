using FaceSystem.UserModel;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.ObjectModel;

namespace FaceSystem.ViewModel
{
    public class PromptViewModel : ViewModelBase
    {
        public PromptViewModel()
        {
            ////if (IsInDesignMode)
            ////{
            ////    // Code runs in Blend --> create design time data.
            ////}
            ////else
            ////{
            ////   
            ///
            /// UserInfos// Code runs "for real"
            ////}
            ///

            //查询
            FindCommand = new RelayCommand(() =>
            {

            });

            IDCardInfos = new ObservableCollection<IDCardInfo>();

            

        }
        static IDCardInfo iDCardInfo;
        

        /// <summary>
        /// 添加用户
        /// </summary>
        public void AddUser(IDCardInfo entity)
        {
            if (entity != null)
            {
                _iDCardInfo.Add(entity);
                RaisePropertyChanged("IDCardInfos");
            }
        }

        public void SetIDCardInfo(IDCardInfo entity)
        {
            iDCardInfo = entity;
            Console.WriteLine(iDCardInfo.Name);
        }

        public IDCardInfo GetIDCardInfo()
        {
            return iDCardInfo;

        }

        public void Clear()
        {
            iDCardInfo = new IDCardInfo();
        }

        #region 用户实体
        /// <summary>
        /// 用户数据集合
        /// </summary>
        private ObservableCollection<IDCardInfo> _iDCardInfo;
        public ObservableCollection<IDCardInfo> IDCardInfos
        {
            get
            {
                return _iDCardInfo;
            }
            set
            {
                _iDCardInfo = value;
                RaisePropertyChanged("IDCardInfos");
            }
        }



        #endregion

        /// <summary>
        /// 查询
        /// </summary>
        public RelayCommand FindCommand
        {
            get;
            private set;
        }

    }
}
