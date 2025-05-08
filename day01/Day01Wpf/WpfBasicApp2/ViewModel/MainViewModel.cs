﻿using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfBasicApp2.ViewModel
{
    internal class MainViewModel : INotifyPropertyChanged
    {
        public MainViewModel()
        {
            LoadControlFromDb();
            LoadGridFromDb();
        }
        public event PropertyChangedEventHandler? PropertyChanged;

        private void LoadControlFromDb()
        {
            // 1. 연결 문자열(DB 연결 문자열은 필수)
            string connectionString = "Server=localhost;Database=bookrentalshop;Uid=root;Pwd=12345;Charset=utf8;";
            // 2. 사용 쿼리
            string query = "SELECT division, names FROM divtbl;";

            // Dictionary나 KeyValuePair 둘 다 상관없음
            List<KeyValuePair<string, string>> divisions = new List<KeyValuePair<string, string>>();

            // 3. DB 연결, 명령, 리더
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    MySqlDataReader reader = cmd.ExecuteReader(); // 데이터 가져올 때 사용

                    while (reader.Read())
                    {
                        var division = reader.GetString("division");
                        var names = reader.GetString("names");

                        divisions.Add(new KeyValuePair<string, string>(division, names));
                    }
                }
                catch (MySqlException ex)
                {
                    // 나중에
                }
            } // conn.Close() 자동 발생

        }

        private void LoadGridFromDb()
        {
            // 1. 연결 문자열(DB 연결 문자열은 필수)
            string connectionString = "Server=localhost;Database=bookrentalshop;Uid=root;Pwd=12345;Charset=utf8;";
            // 2. 사용 쿼리, 기본 쿼리로 먼저 작업 후 필요한 실제 쿼리로 변경
            string query = @"SELECT b.Idx, b.Author, b.Division, b.Names, b.ReleaseDate, b.ISBN, b.Price, d.Names AS dNames
                               FROM bookstbl AS b, divtbl AS d
                              WHERE b.Division = d.Division
                              ORDER BY b.idx";

            // 3. DB 연결, 명령, 리더
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    MySqlDataAdapter adapter = new MySqlDataAdapter(query, conn);
                    DataTable dt = new DataTable();
                    adapter.Fill(dt); // 데이터 가져올 때 사용
                }
                catch (MySqlException ex)
                {
                    // 나중에
                }
            } // conn.Close() 자동 발생
        }
    }
}
