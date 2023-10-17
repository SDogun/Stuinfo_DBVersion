using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using MySql.Data.MySqlClient;

namespace ㅁㅁㅇ
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        //시작할때 자동저장 온오프 체크

        string Strcoon = "Server= 115.85.181.212;Database=s5846471;Uid=s5846471;Pwd=s5846471;charset=utf8";

        private void Form1_Load(object sender, EventArgs e)
        {
            // 성별 comboBox 초기화
            학1성별.Items.Add("남자");
            학1성별.Items.Add("여자");
            학2성별.Items.Add("남자");
            학2성별.Items.Add("여자");
            학3성별박스.Items.Add("남자");
            학3성별박스.Items.Add("여자");

            // 저장된 학생수 계산
            CheckStuInfo();


            //자동 정보 로드 여부 확인
            if (File.Exists("자동저장"))
            {
                LoadStudentInfo();
                자동저장체크.Checked = true;
            }

        }

        //학생정보 불러오기 버튼 누를시
        private void BT_LoadStuintfo_Click(object sender, EventArgs e)
        {
            LoadStudentInfo();
            CheckStuInfo();
        }



        //학생 저장수 체크 
        private void CheckStuInfo()
        {
            int SaveCountStu = 0;
            for (int i = 1; i < 4; i++)
            {
                if (DoesStudentRecordExist(i))
                {
                    SaveCountStu++;
                }
            }
            저장학생알림.Text = SaveCountStu.ToString();
        }


        //학생저장이 얼마나 포함되어 있는지 확인 
        private bool DoesStudentRecordExist(int studentNumber)
        {
            try
            {
                using (MySqlConnection connection = new MySqlConnection(Strcoon))
                {
                    connection.Open();
                    using (MySqlCommand cmd = new MySqlCommand("SELECT 1 FROM student WHERE student_number = @StudentNumber", connection))
                    {
                        cmd.Parameters.AddWithValue("@StudentNumber", studentNumber);
                        object result = cmd.ExecuteScalar();
                        return result != null;
                    }
                }
            }
            catch (MySqlException ex)
            {
                // MySQL 예외 처리
                MessageBox.Show("MySQL 오류: " + ex.Message, "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false; // 오류 발생 시 레코드가 존재하지 않는 것으로 처리
            }
            catch (Exception ex)
            {
                // 그 외 예외 처리
                MessageBox.Show("오류: " + ex.Message, "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false; // 오류 발생 시 레코드가 존재하지 않는 것으로 처리
            }
        }

        //학생정보저장
        private void SaveStudentInfo(int studentNumber, string name, string Stuid, string Gender, string memo)
        {
            using (MySqlConnection connection = new MySqlConnection(Strcoon))
            {
                connection.Open();
                using (MySqlCommand cmd = new MySqlCommand("INSERT INTO student VALUES (@id, @Name, @Student_number, @Gender)", connection))
                {
                    cmd.Parameters.AddWithValue("@id", studentNumber);
                    cmd.Parameters.AddWithValue("@Name", name);
                    cmd.Parameters.AddWithValue("@Student_number", Stuid);
                    cmd.Parameters.AddWithValue("@gender", Gender);
                    cmd.ExecuteNonQuery();
                }
            }
        }


        //학생정보 불러오는 함수
        private void LoadStudentInfo()
        {
            for (int i = 1; i < 4; i++)
            {
                if (DoesStudentRecordExist(i))
                {
                    using (MySqlConnection connection = new MySqlConnection(Strcoon))
                    {
                        connection.Open();
                        using (MySqlCommand cmd = new MySqlCommand("SELECT name, Student_number, Gender, Memo FROM Students WHERE StudentNumber = @StudentNumber", connection))
                        {
                            cmd.Parameters.AddWithValue("@StudentNumber", i);
                            using (MySqlDataReader reader = cmd.ExecuteReader())
                            {
                                if (reader.Read())
                                {
                                    string name = reader["Name"].ToString();
                                    string Stuid = reader["Stuid"].ToString();
                                    string Gender = reader["Gender"].ToString();
                                    string memo = reader["Memo"].ToString();

                                    // 해당 정보를 학생 컨트롤에 채우는 코드 작성
                                    if (i == 1)
                                    {
                                        학1이름입력.Text = name;
                                        학1학번입력.Text = Stuid;
                                        학1성별.Text = Gender;
                                        학1메모인풋.Text = memo;
                                    }
                                    else if (i == 2)
                                    {
                                        학2이름입력.Text = name;
                                        학2학번입력.Text = Stuid;
                                        학2성별.Text = Gender;
                                        학2메모인풋.Text = memo;
                                    }
                                    else if (i == 3)
                                    {
                                        학3이름입력.Text = name;
                                        학3학번입력.Text = Stuid;
                                        학3성별박스.Text = Gender;
                                        학3메모입력.Text = memo;
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }


        //자동저장 유/무
        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            //boolean으로 하여 체크박스가 체크되어있으면 
            if (자동저장체크.Checked == true)
            {
                SaveStudentInfo(1, 학1이름입력.Text, 학1학번입력.Text, 학1성별.Text, 학1메모인풋.Text);
                SaveStudentInfo(2, 학2이름입력.Text, 학2학번입력.Text, 학2성별.Text, 학2메모인풋.Text);
                SaveStudentInfo(3, 학3이름입력.Text, 학3학번입력.Text, 학3성별박스.Text, 학3메모입력.Text);

            }
            else if (자동저장체크.Checked == false)
            {
                File.Delete("자동저장");
            }

        }


        private void 저장학생알_Click(object sender, EventArgs e)
        {

        }

        private void 학1저장_Click(object sender, EventArgs e)
        {
            String name = 학1이름입력.Text.ToString();
            String Stuid = 학1학번입력.Text.ToString();
            String Gender = 학1성별.Text.ToString();
            String memo = 학1메모인풋.Text.ToString();

            using (MySqlConnection connection = new MySqlConnection(Strcoon))
            {
                connection.Open();
                using (MySqlCommand cmd = new MySqlCommand("INSERT INTO student VALUES (@id, @Name, @Student_number, @Gender)", connection))
                {
                    cmd.Parameters.AddWithValue("@id", 1);
                    cmd.Parameters.AddWithValue("@Name", name);
                    cmd.Parameters.AddWithValue("@Student_number", Stuid);
                    cmd.Parameters.AddWithValue("@gender", Gender);
                    cmd.ExecuteNonQuery();
                }
            }
        }
        private void 학2저장_Click(object sender, EventArgs e)
        {
            string name = 학2이름입력.Text.ToString();
            string id = 학2학번입력.Text.ToString();
            string gender = 학2성별.Text.ToString();
            string memo = 학2메모인풋.Text.ToString();
            using (MySqlConnection connection = new MySqlConnection(Strcoon))
            {
                connection.Open();
                using (MySqlCommand cmd = new MySqlCommand("INSERT INTO student VALUES (@id, @Name, @Student_number, @Gender)", connection))
                {
                    cmd.Parameters.AddWithValue("@id", 2);
                    cmd.Parameters.AddWithValue("@Name", name);
                    cmd.Parameters.AddWithValue("@Student_number", id);
                    cmd.Parameters.AddWithValue("@gender", gender);
                    cmd.ExecuteNonQuery();
                }
            }

        }
        private void 학3저장_Click(object sender, EventArgs e)
        {
            string name = 학3이름입력.Text.ToString();
            string id = 학3학번입력.Text.ToString();
            string gender = 학3성별박스.Text.ToString();
            string memo = 학3메모입력.Text.ToString();
            using (MySqlConnection connection = new MySqlConnection(Strcoon))
            {
                connection.Open();
                using (MySqlCommand cmd = new MySqlCommand("INSERT INTO student VALUES (@id, @Name, @Student_number, @Gender)", connection))
                {
                    cmd.Parameters.AddWithValue("@id", 3);
                    cmd.Parameters.AddWithValue("@Name", name);
                    cmd.Parameters.AddWithValue("@Student_number", id);
                    cmd.Parameters.AddWithValue("@gender", gender);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        private void 학1삭제_Click(object sender, EventArgs e)
        {
            string DELQuery = "DELETE FROM student WHERE id = 1";
            using (MySqlConnection connection = new MySqlConnection(Strcoon))
            {

                connection.Open();
                using (MySqlCommand cmd = new MySqlCommand(DELQuery, connection)) 
                {
                    cmd.ExecuteNonQuery();
                }
                
            }
        }

        private void 학2삭제_Click(object sender, EventArgs e)
        {
            string DELQuery = "DELETE FROM student WHERE id = 2";
            using (MySqlConnection connection = new MySqlConnection(Strcoon))
            {
                connection.Open();
                using (MySqlCommand cmd = new MySqlCommand(DELQuery, connection)) 
                {
                    cmd.ExecuteNonQuery();
                }
            }
        }

        private void 학3삭제_Click(object sender, EventArgs e)
        {
            string DELQuery = "DELETE FROM student WHERE id = 3";
            using (MySqlConnection connection = new MySqlConnection(Strcoon))
            {
                connection.Open();
                using (MySqlCommand cmd = new MySqlCommand(DELQuery, connection))
                {

                }
            }
        }
    }
}