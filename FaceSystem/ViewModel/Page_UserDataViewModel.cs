using FaceSystem.UserModel;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System.Collections.ObjectModel;

namespace FaceSystem.ViewModel
{
    /// <summary>
    /// This class contains properties that the main View can data bind to.
    /// <para>
    /// Use the <strong>mvvminpc</strong> snippet to add bindable properties to this ViewModel.
    /// </para>
    /// <para>
    /// You can also use Blend to data bind with the tool's support.
    /// </para>
    /// <para>
    /// See http://www.galasoft.ch/mvvm
    /// </para>
    /// </summary>
    public class Page_UserDataViewModel : ViewModelBase
    {
        /// <summary>
        /// Initializes a new instance of the MainViewModel class.
        /// </summary>
        public Page_UserDataViewModel()
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

            UserInfos = new ObservableCollection<UserInfo>();
        }


        /// <summary>
        /// 添加用户
        /// </summary>
        public void AddUser(UserInfo entity)
        {
            if (entity != null)
            {
                _userInfo.Add(entity);
                RaisePropertyChanged("UserInfos");
            }
        }

        #region 用户实体
        /// <summary>
        /// 用户数据集合
        /// </summary>
        private ObservableCollection<UserInfo> _userInfo;
        public ObservableCollection<UserInfo> UserInfos
        {
            get
            {
                return _userInfo;
            }
            set
            {
                _userInfo = value;
                RaisePropertyChanged("UserInfos");
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