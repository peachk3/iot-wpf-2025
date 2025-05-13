﻿using MahApps.Metro.Controls.Dialogs;
using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfBookRentalShop01.Helpers
{
    /// <summary>
    /// 프로젝트 내에서 같이 쓸 수 있는 공통 클래스
    /// </summary>
    public class Common
    {
        // NLog 인스턴스
        public static readonly Logger LOGGER = LogManager.GetCurrentClassLogger();

        // DB문자열 
        public static readonly string CONNSTR = "Server=localhost;Database=bookrentalshop;Uid=root;Pwd=12345;Charset=utf8;";

        // MahApps.Metro 형태 다이얼로그 코드네이터
        public static IDialogCoordinator DIALOGCOORDINATOR;


    }
}
