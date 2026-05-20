using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bài_Tập_Lớn.DTO;

namespace Bài_Tập_Lớn.DAL
{
    internal class BanBidaDAL
    {
        DBConnection db = DBConnection.Instance;

        // Sinh mã mới
        public string SinhMaMoi()
        {
            string ma = "B01";

            SqlConnection conn = DBConnection.Instance.GetConnection();
            conn.Open();

            string sql = "SELECT TOP 1 MaBan FROM BanBida ORDER BY MaBan DESC";

            SqlCommand cmd = new SqlCommand(sql, conn);

            SqlDataReader reader = cmd.ExecuteReader();

            if (reader.Read())
            {
                string maCu = reader["MaBan"].ToString();

                int so = int.Parse(maCu.Substring(1)) + 1;

                ma = "B" + so.ToString("00");
            }

            conn.Close();

            return ma;
        }

        // Thêm bàn
        public void ThemBan(BanBidaDTO ban)
        {
            SqlConnection conn = db.GetConnection();

            conn.Open();

            string sql = "INSERT INTO BanBida VALUES(@MaBan,@TenBan,@LoaiBan,@GiaTheoGio,@TrangThai)";

            SqlCommand cmd = new SqlCommand(sql, conn);

            cmd.Parameters.AddWithValue("@MaBan", ban.MaBan);
            cmd.Parameters.AddWithValue("@TenBan", ban.TenBan);
            cmd.Parameters.AddWithValue("@LoaiBan", ban.LoaiBan);
            cmd.Parameters.AddWithValue("@GiaTheoGio", ban.GiaTheoGio);
            cmd.Parameters.AddWithValue("@TrangThai", ban.TrangThai);

            cmd.ExecuteNonQuery();

            conn.Close();
        }

        // Cập nhật bàn
        public void CapNhatBan(BanBidaDTO ban)
        {
            SqlConnection conn = db.GetConnection();

            conn.Open();

            string sql = "UPDATE BanBida SET TenBan=@TenBan, LoaiBan=@LoaiBan, GiaTheoGio=@GiaTheoGio, TrangThai=@TrangThai WHERE MaBan=@MaBan";

            SqlCommand cmd = new SqlCommand(sql, conn);

            cmd.Parameters.AddWithValue("@MaBan", ban.MaBan);
            cmd.Parameters.AddWithValue("@TenBan", ban.TenBan);
            cmd.Parameters.AddWithValue("@LoaiBan", ban.LoaiBan);
            cmd.Parameters.AddWithValue("@GiaTheoGio", ban.GiaTheoGio);
            cmd.Parameters.AddWithValue("@TrangThai", ban.TrangThai);

            cmd.ExecuteNonQuery();

            conn.Close();
        }

        // Cập nhật trạng thái
        public void CapNhatTrangThai(string maBan, string trangThai)
        {
            SqlConnection conn = db.GetConnection();

            conn.Open();

            string sql = "UPDATE BanBida SET TrangThai=@TrangThai WHERE MaBan=@MaBan";

            SqlCommand cmd = new SqlCommand(sql, conn);

            cmd.Parameters.AddWithValue("@MaBan", maBan);
            cmd.Parameters.AddWithValue("@TrangThai", trangThai);

            cmd.ExecuteNonQuery();

            conn.Close();
        }

        // Xóa bàn
        public void XoaBan(string maBan)
        {
            SqlConnection conn = db.GetConnection();

            conn.Open();

            string sql = "DELETE FROM BanBida WHERE MaBan=@MaBan";

            SqlCommand cmd = new SqlCommand(sql, conn);

            cmd.Parameters.AddWithValue("@MaBan", maBan);

            cmd.ExecuteNonQuery();

            conn.Close();
        }

        // Lấy tất cả bàn
        public List<BanBidaDTO> LayTatCaBan()
        {
            List<BanBidaDTO> ds = new List<BanBidaDTO>();

            SqlConnection conn = db.GetConnection();

            conn.Open();

            string sql = "SELECT * FROM BanBida";

            SqlCommand cmd = new SqlCommand(sql, conn);

            SqlDataReader reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                BanBidaDTO ban = new BanBidaDTO();

                ban.MaBan = reader["MaBan"].ToString();
                ban.TenBan = reader["TenBan"].ToString();
                ban.LoaiBan = reader["LoaiBan"].ToString();
                ban.GiaTheoGio = Convert.ToDecimal(reader["GiaTheoGio"]);
                ban.TrangThai = reader["TrangThai"].ToString();

                ds.Add(ban);
            }

            conn.Close();

            return ds;
        }

        // Tìm theo mã bàn
        public BanBidaDTO TimTheoMaBan(string maBan)
        {
            BanBidaDTO ban = null;

            SqlConnection conn = db.GetConnection();

            conn.Open();

            string sql = "SELECT * FROM BanBida WHERE MaBan=@MaBan";

            SqlCommand cmd = new SqlCommand(sql, conn);

            cmd.Parameters.AddWithValue("@MaBan", maBan);

            SqlDataReader reader = cmd.ExecuteReader();

            if (reader.Read())
            {
                ban = new BanBidaDTO();

                ban.MaBan = reader["MaBan"].ToString();
                ban.TenBan = reader["TenBan"].ToString();
                ban.LoaiBan = reader["LoaiBan"].ToString();
                ban.GiaTheoGio = Convert.ToDecimal(reader["GiaTheoGio"]);
                ban.TrangThai = reader["TrangThai"].ToString();
            }

            conn.Close();

            return ban;
        }

        // Tìm theo trạng thái
        public List<BanBidaDTO> TimTheoTrangThai(string trangThai)
        {
            List<BanBidaDTO> ds = new List<BanBidaDTO>();

            SqlConnection conn = db.GetConnection();

            conn.Open();

            string sql = "SELECT * FROM BanBida WHERE TrangThai=@TrangThai";

            SqlCommand cmd = new SqlCommand(sql, conn);

            cmd.Parameters.AddWithValue("@TrangThai", trangThai);

            SqlDataReader reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                BanBidaDTO ban = new BanBidaDTO();

                ban.MaBan = reader["MaBan"].ToString();
                ban.TenBan = reader["TenBan"].ToString();
                ban.LoaiBan = reader["LoaiBan"].ToString();
                ban.GiaTheoGio = Convert.ToDecimal(reader["GiaTheoGio"]);
                ban.TrangThai = reader["TrangThai"].ToString();

                ds.Add(ban);
            }

            conn.Close();

            return ds;
        }

        // Tìm theo loại bàn
        public List<BanBidaDTO> TimTheoLoaiBan(string loaiBan)
        {
            List<BanBidaDTO> ds = new List<BanBidaDTO>();

            SqlConnection conn = db.GetConnection();

            conn.Open();

            string sql = "SELECT * FROM BanBida WHERE LoaiBan=@LoaiBan";

            SqlCommand cmd = new SqlCommand(sql, conn);

            cmd.Parameters.AddWithValue("@LoaiBan", loaiBan);

            SqlDataReader reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                BanBidaDTO ban = new BanBidaDTO();

                ban.MaBan = reader["MaBan"].ToString();
                ban.TenBan = reader["TenBan"].ToString();
                ban.LoaiBan = reader["LoaiBan"].ToString();
                ban.GiaTheoGio = Convert.ToDecimal(reader["GiaTheoGio"]);
                ban.TrangThai = reader["TrangThai"].ToString();

                ds.Add(ban);
            }

            conn.Close();

            return ds;
        }
    }
}
