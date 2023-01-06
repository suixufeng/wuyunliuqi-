using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Globalization;
namespace 生成圆圈图片的例子
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
			this.StartPosition = FormStartPosition.CenterScreen;

		}
		public string ConvertCC(DateTime str)//转换公历和农历
        {
			ChineseLunisolarCalendar cncld = new ChineseLunisolarCalendar();
			DateTime dt = new DateTime(str.Year, str.Month, str.Day);
			int year = cncld.GetYear(dt);
			// 是否有闰月,返回正整数（2020年闰4月，返回值为5）
			int flag = cncld.GetLeapMonth(year);
			int month = flag > 0 ? cncld.GetMonth(dt) - 1 : cncld.GetMonth(dt);
			int day = cncld.GetDayOfMonth(dt);
			string nonstr= "农历：" + year + "年" + month + "月" + day + "日";
			return nonstr;
		}
        private void Button1_Click(object sender, EventArgs e)
		{
			
			//DateTime dtnl = dateTimePicker1.ToDateTime(year, month, day, 0, 0, 0, 0);
			//dtnl = flag > 0 ? dtnl.AddMonths(1) : dtnl;
		label5.Text= ConvertCC(dateTimePicker1.Value);
		label6.Text = ConvertCC(dateTimePicker2.Value);
			wuyunliuqi w = new wuyunliuqi();
			//---------------------出生命图
			string g =  w.getGanzhi(dateTimePicker1.Value);		
			string wx = w.getWuxing(dateTimePicker1.Value);
			string s =  w.getSitian(dateTimePicker1.Value);		
			string z = w.getZaiquan(dateTimePicker1.Value);	
			string cz = w.getCurrentZhuqi(dateTimePicker1.Value);
			string ck = w.getCurrentKeqi(dateTimePicker1.Value);
			richTextBox1.Text = "天干：" + g + "\r\n" + "司天：" + s + "\r\n" + "客气：" + ck + "\r\n" + "主运：" + wx + "\r\n" + "主气：" + cz + "\r\n" + "在泉：" + z;
			richTextBox3.Text = "" + yangboNum(g) + "\r\n" + "" + yangboNum(s) + "\r\n" + yangboNum(ck) + "\r\n"  + yangboNum(wx) + "\r\n"  + yangboNum(cz) + "\r\n"  + yangboNum(z);
			//---------------------病图
			string g1 = w.getGanzhi(dateTimePicker2.Value);
			string wx1 = w.getWuxing(dateTimePicker2.Value);
			string s1 =  w.getSitian(dateTimePicker2.Value);
			string z1 =  w.getZaiquan(dateTimePicker2.Value);
			string cz1 =  w.getCurrentZhuqi(dateTimePicker2.Value);
			string ck1 =  w.getCurrentKeqi(dateTimePicker2.Value);
			richTextBox2.Text = "天干：" + g1 + "\r\n" + "司天：" + s1 + "\r\n" + "客气：" + ck1 + "\r\n" + "主运：" + wx1 + "\r\n" + "主气：" + cz1 + "\r\n" + "在泉：" + z1;
			richTextBox4.Text = "" + yangboNum(g1) + "\r\n" + "" + yangboNum(s1) + "\r\n" + yangboNum(ck1) + "\r\n" + yangboNum(wx1) + "\r\n" + yangboNum(cz1) + "\r\n" + yangboNum(z1);
			List<string> mtlist = new List<string>();
			//mtlist.Add(g);
			mtlist.Add(wx);
			mtlist.Add(s);
			mtlist.Add(z);
			mtlist.Add(cz);
			mtlist.Add(ck);

			List<string> mtlist1 = new List<string>();
			//mtlist.Add(g1);
			mtlist1.Add(wx1);
			mtlist1.Add(s1);
			mtlist1.Add(z1);
			mtlist1.Add(cz1);
			mtlist1.Add(ck1);

			this.pictureBox1.Image = (Image)new Bitmap(w.DrwaCircleQL(dateTimePicker1.Value, mtlist));
			this.pictureBox2.Image = (Image)new Bitmap(w.DrwaCircleQL(dateTimePicker2.Value, mtlist1));

		}
		public string  yangboNum(string str)//转换为李阳波数字简化
        {
			string num = "";
			//"厥阴风木", "少阴君火", "少阳相火", "太阴湿土", "阳明燥金", "太阳寒水"
	if(str.Contains("木"))
            {
				num = "410";

			}
			if (str.Contains("金"))
			{
				num = "28";

			}

			if (str.Contains("水"))
			{
				num = "39";

			}

			if (str.Contains("火"))
			{

				//if (str.Contains("少阳相火"))
				//{
				//	num = "17";

				//}
				//else if (str.Contains("少阴君火"))
				//{
				//	num = "115";

				//}
				//else
				//{
				//	num = "?";

				//}
				num = "115";
			}

			
			if (str.Contains("土"))
			{
				num = "126";

			}
			if (str.Contains("太过"))
			{
				num = num+"∧";

			}
			if (str.Contains("不及"))
			{
				num = num+ "∨";

			}


			return num;
        }

		/// <summary>
		/// 画圆圈
		/// </summary>
		/// <param name="bmp_mmm">初始大圈传入null</param>
		/// <param name="默认画布宽">初始大圈传入0</param>
		/// <param name="默认画布高">初始大圈传入0</param>
		/// <param name="中心坐标"></param>
		/// <param name="半径"></param>
		/// <param name="线粗"></param>
		/// <param name="颜色"></param>
		/// <returns></returns>
		public Hashtable 画圈圈(ref Bitmap bmp_mmm,int 默认画布宽,int 默认画布高,Point 中心坐标,int 半径,int 线粗, Color 颜色)
        {
            //初始化返回值, 本次画的圆圈的， 上，右，下，左四个顶点坐标
            Hashtable HT_MMM = new Hashtable();
            HT_MMM["北"] = new Point(中心坐标.X, 中心坐标.Y - 半径);
            HT_MMM["南"] = new Point(中心坐标.X, 中心坐标.Y + 半径);
            HT_MMM["西"] = new Point(中心坐标.X - 半径, 中心坐标.Y);
            HT_MMM["东"] = new Point(中心坐标.X + 半径, 中心坐标.Y);
            HT_MMM["中"] = new Point(中心坐标.X, 中心坐标.Y);

            //空画布，先生成一个白色背景的Bitmap(bmp_mmm)
            if (bmp_mmm == null)
            {
                //bmp_mmm = new Bitmap(默认画布宽, 默认画布高);
                //using (Graphics g_mmm = Graphics.FromImage(bmp_mmm))
                //{
                //    g_mmm.SmoothingMode = SmoothingMode.HighQuality;  //图片柔顺模式选择
                //    g_mmm.InterpolationMode = InterpolationMode.HighQualityBicubic;//高质量
                //    g_mmm.CompositingQuality = CompositingQuality.HighQuality;//再加一点

                //    g_mmm.Clear(Color.White);
                //    g_mmm.Save();
                //    g_mmm.Dispose();
                //}
            }

            else
            {
                using (Graphics g_mmm = Graphics.FromImage(bmp_mmm))
                {
                    g_mmm.SmoothingMode = SmoothingMode.HighQuality;  //图片柔顺模式选择
                    g_mmm.InterpolationMode = InterpolationMode.HighQualityBicubic;//高质量
                    g_mmm.CompositingQuality = CompositingQuality.HighQuality;//再加一点

                    g_mmm.DrawEllipse(new Pen(颜色, 线粗), new Rectangle(中心坐标.X - 半径, 中心坐标.Y - 半径, 半径 * 2, 半径 * 2));
                    g_mmm.Save();
                    g_mmm.Dispose();
                }
            }
            //正常画圈
         


            return HT_MMM;
        }
        /// <summary>
        /// 获取文字占用的空间大小
        /// </summary>
        /// <param name="bmp_mmm"></param>
        /// <param name="text"></param>
        /// <param name="font"></param>
        /// <returns></returns>
        public SizeF Text_Size(ref Bitmap bmp_mmm, string text, Font font)
        {
            SizeF sizef;
            using (Graphics g = Graphics.FromImage(bmp_mmm))
            {
                StringFormat format = new StringFormat(StringFormatFlags.NoClip);
                //计算绘制文字所需的区域大小（根据宽度计算长度），重新创建矩形区域绘图
                sizef = g.MeasureString(text, font, PointF.Empty, format);
            }
            return sizef;

        }

        /// <summary>
        /// 写文字
        /// </summary>
        /// <param name="bmp_mmm"></param>
        /// <param name="text"></param>
        /// <param name="X"></param>
        /// <param name="Y"></param>
        /// <param name="font"></param>
        /// <param name="fontcolor"></param>
        /// <param name="backColor"></param>
        public void TextToBitmap(ref Bitmap bmp_mmm, string text, int X, int Y, Font font, Color fontcolor, Color backColor)
        {
            using (Graphics g = Graphics.FromImage(bmp_mmm))
            {
                StringFormat format = new StringFormat(StringFormatFlags.NoClip);

                //计算绘制文字所需的区域大小（根据宽度计算长度），重新创建矩形区域绘图
                SizeF sizef = g.MeasureString(text, font, PointF.Empty, format);

                int width = (int)(sizef.Width + 1);
                int height = (int)(sizef.Height + 1);
                Rectangle rect = new Rectangle(X, Y, width, height);

                //使用ClearType字体功能
                g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;
                g.FillRectangle(new SolidBrush(backColor), rect);
                g.DrawString(text, font, Brushes.Black, rect, format);
            }

        }


    }

	class wuyunliuqi
	{
		string[] GAN = { "甲", "乙", "丙", "丁", "戊", "己", "庚", "辛", "壬", "癸" };
		string[] ZHI = { "子", "丑", "寅", "卯", "辰", "已", "午", "未", "申", "酉", "戌", "亥" };
		string[] sitianAll = { "少阴君火", "太阴湿土", "少阳相火", "阳明燥金", "太阳寒水", "厥阴风木" };
		string[] wuxingAll = { "木", "火", "土", "金", "水" };
		string[] liuqiAll = { "厥阴风木", "少阴君火", "少阳相火", "太阴湿土", "阳明燥金", "太阳寒水" };
		public string getGanzhi(DateTime dt)//获取干支
		{
			int year = Convert.ToInt32(dt.Year.ToString());
			var i_gan = (year - 1984 + 6000) % 10;
			var i_zhi = (year - 1984 + 6000) % 12;
			var ganzhi = GAN[i_gan] + ZHI[i_zhi];

			return ganzhi;
		}
		public string getWuxing(DateTime dt)//获取干支
		{
			var gan_offset = 2;
			int year = Convert.ToInt32(dt.Year.ToString());
			var i_gan = (year - 1984 + 6000) % 10;
			var i_zhi = (year - 1984 + 6000) % 12;
			var idx_gan = (i_gan + gan_offset) % 5;
			var wuxing = wuxingAll[idx_gan];
			var guoji = i_gan % 2 == 0 ? "太过" : "不及";
			return wuxing + "-" + guoji;
		}
		public string getSitian(DateTime dt)//获取司天
		{
			int year = Convert.ToInt32(dt.Year.ToString());
			var i_zhi = (year - 1984 + 6000) % 12;
			var idx_zhi = i_zhi % 6;
			var sitian = sitianAll[idx_zhi];
			return sitian;
		}
		public string getZaiquan(DateTime dt)//获取在泉
		{
			int year = Convert.ToInt32(dt.Year.ToString());
			var i_zhi = (year - 1984 + 6000) % 12;
			var idx_zhi = i_zhi % 6;
			var zaiquan = sitianAll[(idx_zhi + 3) % 6];
			return zaiquan;
		}
		public List<string> getZhuqi(DateTime dt)//获取一年的主气列表
		{
			int year = Convert.ToInt32(dt.Year.ToString());
			string[] zhuqi = new string[6];
			for (var i = 0; i < 6; i++)
			{
				zhuqi[i] = liuqiAll[i];
				//zhuqi[i].Dump();
			}
			return zhuqi.ToList();
		}
		public List<string> getKeqi(DateTime dt)//获取一年的客气列表
		{
			int year = Convert.ToInt32(dt.Year.ToString());
			var i_zhi = (year - 1984 + 6000) % 12;
			var idx_zhi = i_zhi % 6;
			string[] keqi = new string[6];
			for (var i = 0; i < 6; i++)
			{
				keqi[i] = sitianAll[(idx_zhi - 2 + 6 + i) % 6];
				//keqi[i].Dump();
			}
			return keqi.ToList();
		}
		public string getCurrentZhuqi(DateTime mday)//获取当前月份的主气
		{
			int year = Convert.ToInt32(mday.Year.ToString());
			int month = Convert.ToInt32(mday.Month.ToString());
			int day = Convert.ToInt32(mday.Day.ToString());
			string[] zhuqi = new string[6];
			string qi = "";
			string str = "";
			//		< th > 六气 </ th >
			//	< th class="center">初之气<br>01.21 ~ 03.21</th>
			//	<th class="center">二之气<br>03.21 ~ 05.21</th>
			//	<th class="center">三之气<br>05.21 ~ 07.23</th>
			//
			//	<th class="center">四之气<br>07.23 ~ 09.23</th>
			//	<th class="center">五之气<br>09.23 ~ 11.23</th>
			//	<th class="center">终之气<br>11.23 ~ 01.21</th>
			string cudate = (year.ToString() + "." + 01.21);
			DateTime a = Convert.ToDateTime(cudate);
			if (mday > a && mday < a.AddMonths(2))
			{
				qi = "初之气";

			}
			else if (mday > a.AddMonths(2) && mday < a.AddMonths(4))
			{
				qi = "二之气";
			}
			else if (mday > a.AddMonths(4) && mday < a.AddMonths(6))
			{
				qi = "三之气";
			}
			else if (mday > a.AddMonths(6) && mday < a.AddMonths(8))
			{
				qi = "四之气";
			}
			else if (mday > a.AddMonths(8) && mday < a.AddMonths(10))
			{
				qi = "五之气";
			}
			//else if (mday > a.AddMonths(10) && mday < a.AddMonths(12))
			else 
			{
				qi = "终之气";
			}
			switch (qi)
			{
				case "初之气":
					str = "厥阴风木";
					break;
				case "二之气":
					str = "少阴君火";
					break;
				case "三之气":
					str = "少阳相火";
					break;
				case "四之气":
					str = "太阴湿土";
					break;
				case "五之气":
					str = "阳明燥金";
					break;
				case "终之气":
					str = "太阳寒水";
					break;
			}

			return str;
		}

		public string getCurrentKeqi(DateTime mday)//获取当前月份的客气
		{
			int year = Convert.ToInt32(mday.Year.ToString());
			int month = Convert.ToInt32(mday.Month.ToString());
			int day = Convert.ToInt32(mday.Day.ToString());
			var i_zhi = (year - 1984 + 6000) % 12;
			var idx_zhi = i_zhi % 6;
			string[] keqi = new string[6];
			string qi = "";
			int num = 0;
			string cudate = (year.ToString() + "." + 01.21);
			DateTime a = Convert.ToDateTime(cudate);
			string zhu = getCurrentZhuqi(mday);
			switch (zhu)
			{
				case "厥阴风木":
					num = 0;
					break;
				case "少阴君火":
					num = 1;
					break;
				case "少阳相火":
					num = 2;
					break;
				case "太阴湿土":
					num = 3;
					break;
				case "阳明燥金":
					num = 4;
					break;
				case "太阳寒水":
					num = 5;
					break;
			}
			for (var i = 0; i < 6; i++)
			{
				keqi[i] = sitianAll[(idx_zhi - 2 + 6 + i) % 6];
				//keqi[i].Dump();
			}
			return keqi[num];//根据当前月份主气所处的区间，推算出当前月份的客气


		}
		public static string getYear(int year) //获取全部五运六气信息，算法不好废弃
		{
			string[] tianganAll = { "甲", "乙", "丙", "丁", "戊", "己", "庚", "辛", "壬", "癸" };
			string[] dizhiAll = { "子", "丑", "寅", "卯", "辰", "巳", "午", "未", "申", "酉", "戌", "亥" };

			//1984为甲子年，以此为基础进行计算；天干10年一轮回，地支12年一轮回，60轮回后天干地支完全相同
			//var Todayyear=DateTime.Now.Year;
			//var Todayyear = 1986;
			var cha = year - 1984;
			if (cha < 0)
			{
				cha = year - 1924;
			}
			int yu1 = 0;
			string tiangan = "";
			string dizhi = "";
			if (cha > 0)
			{

			}
			if (cha > 10)
			{
				yu1 = cha % 10;
				tiangan = tianganAll[yu1];
			}
			else
			{
				tiangan = tianganAll[cha];
			}
			if (cha > 12)
			{
				yu1 = cha % 12;
				dizhi = dizhiAll[yu1];
			}
			else
			{
				dizhi = dizhiAll[cha];
			}

			return tiangan + dizhi;
		}
		/// <summary>
		/// 五行脏腑气立审计统一图
		/// </summary>
		/// <param name="dt"></param>
		/// <returns></returns>
		public string  DrwaCircle(DateTime dt)
		{
			//图片目录
			string rootpath = Environment.CurrentDirectory + "\\生成的图片\\";
			if (!Directory.Exists(rootpath))
			{
				Directory.CreateDirectory(rootpath);
			}
			//最终图片地址
			string mmm_img = rootpath + dt.ToString("yyyy-MM-dd_HH_mm_ss_fff") + ".png";

			//开始画中心大图
			Bitmap bmp_mmm = null;
			int 默认画布宽 = 1200;
			int 默认画布高 = 1200;
			int 初始半径 = 100;
			Point 画布中心点 = new Point(默认画布宽 / 2, 默认画布高 / 2);
			int 线粗 = 5;
			Color 颜色 = Color.Black;//黑色

			if (bmp_mmm == null)
			{
				bmp_mmm = new Bitmap(默认画布宽, 默认画布高);
				using (Graphics g_mmm = Graphics.FromImage(bmp_mmm))
				{
					g_mmm.SmoothingMode = SmoothingMode.HighQuality;  //图片柔顺模式选择
					g_mmm.InterpolationMode = InterpolationMode.HighQualityBicubic;//高质量
					g_mmm.CompositingQuality = CompositingQuality.HighQuality;//再加一点

					g_mmm.Clear(Color.White);
					g_mmm.Save();
					g_mmm.Dispose();
				}
			}
			Hashtable HT_MMM = 画圈圈(ref bmp_mmm, 默认画布宽, 默认画布高, 画布中心点, 初始半径, 线粗, 颜色);//中间圆圈

			Hashtable HT_MMM1 = 画圈圈(ref bmp_mmm, 默认画布宽, 默认画布高, new Point(画布中心点.X, 画布中心点.Y + 3 * 初始半径), 初始半径, 线粗, 颜色);//下方大圈
			Hashtable HT_MMM2 = 画圈圈(ref bmp_mmm, 默认画布宽, 默认画布高, new Point(画布中心点.X, 画布中心点.Y - 3 * 初始半径), 初始半径, 线粗, 颜色);//上方方大圈
			Hashtable HT_MMM3 = 画圈圈(ref bmp_mmm, 默认画布宽, 默认画布高, new Point(画布中心点.X - 3 * 初始半径, 画布中心点.Y), 初始半径, 线粗, 颜色);//左方大圈
			Hashtable HT_MMM4 = 画圈圈(ref bmp_mmm, 默认画布宽, 默认画布高, new Point(画布中心点.X + 3 * 初始半径, 画布中心点.Y), 初始半径, 线粗, 颜色);//右方方大圈
																													 //开始画小圈
																													 //--------------------center
																													 // 画圈圈(ref bmp_mmm, 0, 0, (Point)(HT_MMM["北"]), 30, 线粗, 颜色);

			//画圈圈(ref bmp_mmm, 0, 0, (Point)(HT_MMM["南"]), 30, 线粗, 颜色);

			//画圈圈(ref bmp_mmm, 0, 0, (Point)(HT_MMM["西"]), 30, 线粗, 颜色);

			画圈圈(ref bmp_mmm, 0, 0, (Point)(HT_MMM["东"]), 30, 线粗, 颜色);

			画圈圈(ref bmp_mmm, 0, 0, (Point)(HT_MMM["中"]), 30, 线粗, 颜色);

			//-------------------------down

			画圈圈(ref bmp_mmm, 0, 0, (Point)(HT_MMM1["北"]), 30, 线粗, 颜色);
			画圈圈(ref bmp_mmm, 0, 0, (Point)(HT_MMM1["南"]), 30, 线粗, 颜色);

			//-----------------------up


			画圈圈(ref bmp_mmm, 0, 0, (Point)(HT_MMM2["北"]), 30, 线粗, 颜色);
			画圈圈(ref bmp_mmm, 0, 0, (Point)(HT_MMM2["北"]), 20, 线粗, 颜色);
			画圈圈(ref bmp_mmm, 0, 0, (Point)(HT_MMM2["南"]), 30, 线粗, 颜色);
			画圈圈(ref bmp_mmm, 0, 0, (Point)(HT_MMM2["西"]), 30, 线粗, 颜色);

			//-----------------------left

			画圈圈(ref bmp_mmm, 0, 0, (Point)(HT_MMM3["北"]), 30, 线粗, 颜色);
			画圈圈(ref bmp_mmm, 0, 0, (Point)(HT_MMM3["西"]), 30, 线粗, 颜色);
			//------------------------right
			画圈圈(ref bmp_mmm, 0, 0, (Point)(HT_MMM4["东"]), 30, 线粗, 颜色);
			画圈圈(ref bmp_mmm, 0, 0, (Point)(HT_MMM4["中"]), 30, 线粗, 颜色);
			//合适的地方放文字
			Font F = new Font("tahoma", 30);
			string T = "土 脾";
			//必须先获得文字大小,因为不同像素，不同字体，文字大小不同，影响位置计算。
			SizeF s = Text_Size(ref bmp_mmm, T, F);
			//定义文字的具体问题，比如这里是北顶点偏下一点正中间
			Point TP = new Point(((Point)(HT_MMM["中"])).X - Convert.ToInt32(s.Width / 4), ((Point)(HT_MMM["中"])).Y + 40);
			TextToBitmap(ref bmp_mmm, T, TP.X, TP.Y, new Font("tahoma", 20), 颜色, Color.White);

			string T1 = "胃";
			Point WP = new Point(((Point)(HT_MMM["东"])).X + 10, ((Point)(HT_MMM["东"])).Y + 40);
			TextToBitmap(ref bmp_mmm, T1, WP.X, WP.Y, new Font("tahoma", 20), 颜色, Color.White);

			string T2 = "金 肺";
			Point FP = new Point(((Point)(HT_MMM4["中"])).X - Convert.ToInt32(s.Width / 4), ((Point)(HT_MMM4["中"])).Y + 40);
			TextToBitmap(ref bmp_mmm, T2, FP.X, FP.Y, new Font("tahoma", 20), 颜色, Color.White);

			string T3 = "大肠";
			Point DP = new Point(((Point)(HT_MMM4["东"])).X + 10, ((Point)(HT_MMM4["东"])).Y + 40);
			TextToBitmap(ref bmp_mmm, T3, DP.X, DP.Y, new Font("tahoma", 20), 颜色, Color.White);

			string T4 = "胆";
			Point DAP = new Point(((Point)(HT_MMM3["北"])).X - Convert.ToInt32(s.Width / 4), ((Point)(HT_MMM3["北"])).Y + 40);
			TextToBitmap(ref bmp_mmm, T4, DAP.X, DAP.Y, new Font("tahoma", 20), 颜色, Color.White);
			string T5 = "肝";
			Point GP = new Point(((Point)(HT_MMM3["西"])).X - Convert.ToInt32(s.Width / 4), ((Point)(HT_MMM3["西"])).Y + 40);
			TextToBitmap(ref bmp_mmm, T5, GP.X, GP.Y, new Font("tahoma", 20), 颜色, Color.White);
			string TM = "木";
			Point MMP = new Point(((Point)(HT_MMM3["中"])).X, ((Point)(HT_MMM3["中"])).Y);
			TextToBitmap(ref bmp_mmm, TM, MMP.X, MMP.Y, new Font("tahoma", 20), 颜色, Color.White);

			string T6 = "肾";
			Point SP = new Point(((Point)(HT_MMM1["北"])).X - Convert.ToInt32(s.Width / 4), ((Point)(HT_MMM1["北"])).Y + 40);
			TextToBitmap(ref bmp_mmm, T6, SP.X, SP.Y, new Font("tahoma", 20), 颜色, Color.White);
			string T7 = "膀胱";
			Point PP = new Point(((Point)(HT_MMM1["南"])).X - Convert.ToInt32(s.Width / 4), ((Point)(HT_MMM1["南"])).Y + 40);
			TextToBitmap(ref bmp_mmm, T7, PP.X, PP.Y, new Font("tahoma", 20), 颜色, Color.White);
			string T8 = "水";
			Point MP = new Point(((Point)(HT_MMM1["中"])).X, ((Point)(HT_MMM1["中"])).Y);
			TextToBitmap(ref bmp_mmm, T8, MP.X, MP.Y, new Font("tahoma", 20), 颜色, Color.White);

			string T9 = "心、三焦";
			Point XP = new Point(((Point)(HT_MMM2["北"])).X - Convert.ToInt32(s.Width / 4), ((Point)(HT_MMM2["北"])).Y + 40);
			TextToBitmap(ref bmp_mmm, T9, XP.X, XP.Y, new Font("tahoma", 20), 颜色, Color.White);
			//string TA = "三焦";
			//Point SJP = new Point(((Point)(HT_MMM2["北"])).X - Convert.ToInt32(s.Width / 2), ((Point)(HT_MMM2["北"])).Y + 40);
			//TextToBitmap(ref bmp_mmm, TA, SJP.X, SJP.Y, new Font("tahoma", 20), 颜色, Color.White);
			string TB = "小肠";
			Point XCP = new Point(((Point)(HT_MMM2["南"])).X, ((Point)(HT_MMM2["南"])).Y + 40);
			TextToBitmap(ref bmp_mmm, TB, XCP.X, XCP.Y, new Font("tahoma", 20), 颜色, Color.White);
			string TC = "心包";
			Point XXP = new Point(((Point)(HT_MMM2["西"])).X - 60, ((Point)(HT_MMM2["西"])).Y + 40);
			TextToBitmap(ref bmp_mmm, TC, XXP.X, XXP.Y, new Font("tahoma", 20), 颜色, Color.White);
			string TD = "火";
			Point HP = new Point(((Point)(HT_MMM2["中"])).X, ((Point)(HT_MMM2["中"])).Y);
			TextToBitmap(ref bmp_mmm, TD, HP.X, HP.Y, new Font("tahoma", 20), 颜色, Color.White);
			//保存图片
			if (!File.Exists(mmm_img))
			{
				bmp_mmm.Save(mmm_img, ImageFormat.Png);

			}
				//bmp_mmm.Save(mmm_img, ImageFormat.Png);
            bmp_mmm.Dispose();//最终画完，一定要用这个关闭
							  //System.Web.HttpResponse Response = System.Web.HttpContext.Current.Response;
							  //Response.Clear();
							  //Response.BufferOutput = true; //特别注意 
							  //Response.Cache.SetExpires(DateTime.Now.AddMilliseconds(-1));//特别注意 
							  //Response.Cache.SetCacheability(HttpCacheability.NoCache);//特别注意 
							  //Response.AppendHeader("Pragma", "No-Cache"); //特别注意 
							  //MemoryStream ms = new MemoryStream();

			//bmp_mmm.Save(ms, ImageFormat.Jpeg);
			//Response.ClearContent();
			//Response.ContentType = "image/PNG";
			//Response.BinaryWrite(ms.ToArray());
			//Response.End();
			//ms.Close();
			//ms = null;
			//bmp_mmm.Dispose();
			//bmp_mmm = null;
			return mmm_img;
			

		}
		/// <summary>
		/// 气立时图
		/// </summary>
		/// <param name="dt"></param>
		/// <returns></returns>
		public string DrwaCircleQL(DateTime dt,List<string> lstr)
		{
			//图片目录
			string rootpath = Environment.CurrentDirectory + "\\生成的图片\\";
			if (!Directory.Exists(rootpath))
			{
				Directory.CreateDirectory(rootpath);
			}
			//最终图片地址
			string mmm_img = rootpath + dt.ToString("yyyy-MM-dd_HH_mm_ss_fff") + ".png";

			//开始画中心大图
			Bitmap bmp_mmm = null;
			int 默认画布宽 = 1200;
			int 默认画布高 = 1200;
			int 初始半径 = 100;
			Point 画布中心点 = new Point(默认画布宽 / 2, 默认画布高 / 2);
			int 线粗 = 5;
			Color 颜色 = Color.Black;//黑色

			if (bmp_mmm == null)
			{
				bmp_mmm = new Bitmap(默认画布宽, 默认画布高);
				using (Graphics g_mmm = Graphics.FromImage(bmp_mmm))
				{
					g_mmm.SmoothingMode = SmoothingMode.HighQuality;  //图片柔顺模式选择
					g_mmm.InterpolationMode = InterpolationMode.HighQualityBicubic;//高质量
					g_mmm.CompositingQuality = CompositingQuality.HighQuality;//再加一点

					g_mmm.Clear(Color.White);
					g_mmm.Save();
					g_mmm.Dispose();
				}
			}
			Hashtable HT_MMM = 画圈圈(ref bmp_mmm, 默认画布宽, 默认画布高, 画布中心点, 初始半径, 线粗, 颜色);//中间圆圈

			Hashtable HT_MMM1 = 画圈圈(ref bmp_mmm, 默认画布宽, 默认画布高, new Point(画布中心点.X, 画布中心点.Y + 3 * 初始半径), 初始半径, 线粗, 颜色);//下方大圈
			Hashtable HT_MMM2 = 画圈圈(ref bmp_mmm, 默认画布宽, 默认画布高, new Point(画布中心点.X, 画布中心点.Y - 3 * 初始半径), 初始半径, 线粗, 颜色);//上方方大圈
			Hashtable HT_MMM3 = 画圈圈(ref bmp_mmm, 默认画布宽, 默认画布高, new Point(画布中心点.X - 3 * 初始半径, 画布中心点.Y), 初始半径, 线粗, 颜色);//左方大圈
			Hashtable HT_MMM4 = 画圈圈(ref bmp_mmm, 默认画布宽, 默认画布高, new Point(画布中心点.X + 3 * 初始半径, 画布中心点.Y), 初始半径, 线粗, 颜色);//右方方大圈
       
            int a = 60;//小圈半径
			int b= 60;
			int c = 60;
			int d = 60;
			int e = 60;
			int f = 60;

			foreach (var i in lstr)
			{
				if (i== "厥阴风木"||i.Substring(0,1)=="木")
                {
					画圈圈(ref bmp_mmm, 0, 0, (Point)(HT_MMM3["西"]), a, 线粗, 颜色);//中医的南北和地图的是相反的
					a = a - 10;
				}
				if (i == "少阴君火"|| i == "少阳相火" ||i.Substring(0,1) == "火")
				{
					画圈圈(ref bmp_mmm, 0, 0, (Point)(HT_MMM2["北"]), b, 线粗, 颜色);
					b = b - 10;
				}
				//if (i == "少阳相火")
				//{
				//	画圈圈(ref bmp_mmm, 0, 0, (Point)(HT_MMM3["西"]), v, 线粗, 颜色);
				//	v = v - 5;
				//}
				if (i == "太阴湿土" ||i.Substring(0,1) == "土")
				{
					画圈圈(ref bmp_mmm, 0, 0, (Point)(HT_MMM["中"]), c, 线粗, 颜色);
					c = c - 10;
				}
				if (i == "阳明燥金" ||i.Substring(0,1) == "金")
				{
					画圈圈(ref bmp_mmm, 0, 0, (Point)(HT_MMM4["东"]), d, 线粗, 颜色);
					d = d - 10;
				}
				if (i == "太阳寒水" ||i.Substring(0,1) == "水")
				{
					画圈圈(ref bmp_mmm, 0, 0, (Point)(HT_MMM1["南"]), e, 线粗, 颜色);
					e = e - 10;
				}
			}
			lstr.Clear();


            //画圈圈(ref bmp_mmm, 0, 0, (Point)(HT_MMM["东"]), 30, 线粗, 颜色);

            //画圈圈(ref bmp_mmm, 0, 0, (Point)(HT_MMM["中"]), 30, 线粗, 颜色);

            ////-------------------------down

            //画圈圈(ref bmp_mmm, 0, 0, (Point)(HT_MMM1["北"]), 30, 线粗, 颜色);
            //画圈圈(ref bmp_mmm, 0, 0, (Point)(HT_MMM1["南"]), 30, 线粗, 颜色);

            ////-----------------------up


            //画圈圈(ref bmp_mmm, 0, 0, (Point)(HT_MMM2["北"]), 30, 线粗, 颜色);
            //画圈圈(ref bmp_mmm, 0, 0, (Point)(HT_MMM2["北"]), 20, 线粗, 颜色);
            //画圈圈(ref bmp_mmm, 0, 0, (Point)(HT_MMM2["南"]), 30, 线粗, 颜色);
            //画圈圈(ref bmp_mmm, 0, 0, (Point)(HT_MMM2["西"]), 30, 线粗, 颜色);

            ////-----------------------left

            //画圈圈(ref bmp_mmm, 0, 0, (Point)(HT_MMM3["北"]), 30, 线粗, 颜色);
            //画圈圈(ref bmp_mmm, 0, 0, (Point)(HT_MMM3["西"]), 30, 线粗, 颜色);
            ////------------------------right
            //画圈圈(ref bmp_mmm, 0, 0, (Point)(HT_MMM4["东"]), 30, 线粗, 颜色);
            //画圈圈(ref bmp_mmm, 0, 0, (Point)(HT_MMM4["中"]), 30, 线粗, 颜色);
            ////合适的地方放文字
            Font F = new Font("tahoma", 30);
            string T = "土 脾";
            //必须先获得文字大小,因为不同像素，不同字体，文字大小不同，影响位置计算。
            SizeF s = Text_Size(ref bmp_mmm, T, F);
            //定义文字的具体问题，比如这里是北顶点偏下一点正中间
            Point TP = new Point(((Point)(HT_MMM["中"])).X - Convert.ToInt32(s.Width / 4), ((Point)(HT_MMM["中"])).Y + 40);
            TextToBitmap(ref bmp_mmm, T, TP.X, TP.Y, new Font("tahoma", 20), 颜色, Color.White);

            //string T1 = "胃";
            //Point WP = new Point(((Point)(HT_MMM["东"])).X + 10, ((Point)(HT_MMM["东"])).Y + 40);
            //TextToBitmap(ref bmp_mmm, T1, WP.X, WP.Y, new Font("tahoma", 20), 颜色, Color.White);

            string T2 = "金 肺";
            Point FP = new Point(((Point)(HT_MMM4["中"])).X - Convert.ToInt32(s.Width / 4), ((Point)(HT_MMM4["中"])).Y + 40);
            TextToBitmap(ref bmp_mmm, T2, FP.X, FP.Y, new Font("tahoma", 20), 颜色, Color.White);

            //string T3 = "大肠";
            //Point DP = new Point(((Point)(HT_MMM4["东"])).X + 10, ((Point)(HT_MMM4["东"])).Y + 40);
            //TextToBitmap(ref bmp_mmm, T3, DP.X, DP.Y, new Font("tahoma", 20), 颜色, Color.White);

            //string T4 = "胆";
            //Point DAP = new Point(((Point)(HT_MMM3["北"])).X - Convert.ToInt32(s.Width / 4), ((Point)(HT_MMM3["北"])).Y + 40);
            //TextToBitmap(ref bmp_mmm, T4, DAP.X, DAP.Y, new Font("tahoma", 20), 颜色, Color.White);
            string T5 = "肝";
            Point GP = new Point(((Point)(HT_MMM3["西"])).X - Convert.ToInt32(s.Width / 4), ((Point)(HT_MMM3["西"])).Y + 40);
            TextToBitmap(ref bmp_mmm, T5, GP.X, GP.Y, new Font("tahoma", 20), 颜色, Color.White);
            string TM = "木";
            Point MMP = new Point(((Point)(HT_MMM3["中"])).X, ((Point)(HT_MMM3["中"])).Y);
            TextToBitmap(ref bmp_mmm, TM, MMP.X, MMP.Y, new Font("tahoma", 20), 颜色, Color.White);

            //string T6 = "肾";
            //Point SP = new Point(((Point)(HT_MMM1["北"])).X - Convert.ToInt32(s.Width / 4), ((Point)(HT_MMM1["北"])).Y + 40);
            //TextToBitmap(ref bmp_mmm, T6, SP.X, SP.Y, new Font("tahoma", 20), 颜色, Color.White);
            string T7 = "膀胱";
            Point PP = new Point(((Point)(HT_MMM1["南"])).X - Convert.ToInt32(s.Width / 4), ((Point)(HT_MMM1["南"])).Y + 40);
            TextToBitmap(ref bmp_mmm, T7, PP.X, PP.Y, new Font("tahoma", 20), 颜色, Color.White);
            string T8 = "水";
            Point MP = new Point(((Point)(HT_MMM1["中"])).X, ((Point)(HT_MMM1["中"])).Y);
            TextToBitmap(ref bmp_mmm, T8, MP.X, MP.Y, new Font("tahoma", 20), 颜色, Color.White);

            string T9 = "心、三焦";
            Point XP = new Point(((Point)(HT_MMM2["北"])).X - Convert.ToInt32(s.Width / 4), ((Point)(HT_MMM2["北"])).Y + 40);
            TextToBitmap(ref bmp_mmm, T9, XP.X, XP.Y, new Font("tahoma", 20), 颜色, Color.White);
            ////string TA = "三焦";
            ////Point SJP = new Point(((Point)(HT_MMM2["北"])).X - Convert.ToInt32(s.Width / 2), ((Point)(HT_MMM2["北"])).Y + 40);
            ////TextToBitmap(ref bmp_mmm, TA, SJP.X, SJP.Y, new Font("tahoma", 20), 颜色, Color.White);
            //string TB = "小肠";
            //Point XCP = new Point(((Point)(HT_MMM2["南"])).X, ((Point)(HT_MMM2["南"])).Y + 40);
            //TextToBitmap(ref bmp_mmm, TB, XCP.X, XCP.Y, new Font("tahoma", 20), 颜色, Color.White);
            //string TC = "心包";
            //Point XXP = new Point(((Point)(HT_MMM2["西"])).X - 60, ((Point)(HT_MMM2["西"])).Y + 40);
            //TextToBitmap(ref bmp_mmm, TC, XXP.X, XXP.Y, new Font("tahoma", 20), 颜色, Color.White);
            string TD = "火";
            Point HP = new Point(((Point)(HT_MMM2["中"])).X, ((Point)(HT_MMM2["中"])).Y);
            TextToBitmap(ref bmp_mmm, TD, HP.X, HP.Y, new Font("tahoma", 20), 颜色, Color.White);
            //保存图片
            if (!File.Exists(mmm_img))
			{
				bmp_mmm.Save(mmm_img, ImageFormat.Png);

			}
			//bmp_mmm.Save(mmm_img, ImageFormat.Png);
			bmp_mmm.Dispose();//最终画完，一定要用这个关闭
							  //System.Web.HttpResponse Response = System.Web.HttpContext.Current.Response;
							  //Response.Clear();
							  //Response.BufferOutput = true; //特别注意 
							  //Response.Cache.SetExpires(DateTime.Now.AddMilliseconds(-1));//特别注意 
							  //Response.Cache.SetCacheability(HttpCacheability.NoCache);//特别注意 
							  //Response.AppendHeader("Pragma", "No-Cache"); //特别注意 
							  //MemoryStream ms = new MemoryStream();

			//bmp_mmm.Save(ms, ImageFormat.Jpeg);
			//Response.ClearContent();
			//Response.ContentType = "image/PNG";
			//Response.BinaryWrite(ms.ToArray());
			//Response.End();
			//ms.Close();
			//ms = null;
			//bmp_mmm.Dispose();
			//bmp_mmm = null;
			return mmm_img;


		}
		/// <summary>
		/// 画圆圈
		/// </summary>
		/// <param name="bmp_mmm">初始大圈传入null</param>
		/// <param name="默认画布宽">初始大圈传入0</param>
		/// <param name="默认画布高">初始大圈传入0</param>
		/// <param name="中心坐标"></param>
		/// <param name="半径"></param>
		/// <param name="线粗"></param>
		/// <param name="颜色"></param>
		/// <returns></returns>
		public Hashtable 画圈圈(ref Bitmap bmp_mmm, int 默认画布宽, int 默认画布高, Point 中心坐标, int 半径, int 线粗, Color 颜色)
		{
			//初始化返回值, 本次画的圆圈的， 上，右，下，左四个顶点坐标
			Hashtable HT_MMM = new Hashtable();
			HT_MMM["北"] = new Point(中心坐标.X, 中心坐标.Y - 半径);
			HT_MMM["南"] = new Point(中心坐标.X, 中心坐标.Y + 半径);
			HT_MMM["西"] = new Point(中心坐标.X - 半径, 中心坐标.Y);
			HT_MMM["东"] = new Point(中心坐标.X + 半径, 中心坐标.Y);
			HT_MMM["中"] = new Point(中心坐标.X, 中心坐标.Y);

			//空画布，先生成一个白色背景的Bitmap(bmp_mmm)
			if (bmp_mmm == null)
			{
				//bmp_mmm = new Bitmap(默认画布宽, 默认画布高);
				//using (Graphics g_mmm = Graphics.FromImage(bmp_mmm))
				//{
				//    g_mmm.SmoothingMode = SmoothingMode.HighQuality;  //图片柔顺模式选择
				//    g_mmm.InterpolationMode = InterpolationMode.HighQualityBicubic;//高质量
				//    g_mmm.CompositingQuality = CompositingQuality.HighQuality;//再加一点

				//    g_mmm.Clear(Color.White);
				//    g_mmm.Save();
				//    g_mmm.Dispose();
				//}
			}

			else
			{
				using (Graphics g_mmm = Graphics.FromImage(bmp_mmm))
				{
					g_mmm.SmoothingMode = SmoothingMode.HighQuality;  //图片柔顺模式选择
					g_mmm.InterpolationMode = InterpolationMode.HighQualityBicubic;//高质量
					g_mmm.CompositingQuality = CompositingQuality.HighQuality;//再加一点

					g_mmm.DrawEllipse(new Pen(颜色, 线粗), new Rectangle(中心坐标.X - 半径, 中心坐标.Y - 半径, 半径 * 2, 半径 * 2));
					g_mmm.Save();
					g_mmm.Dispose();
				}
			}
			//正常画圈



			return HT_MMM;
		}
		public SizeF Text_Size(ref Bitmap bmp_mmm, string text, Font font)
		{
			SizeF sizef;
			using (Graphics g = Graphics.FromImage(bmp_mmm))
			{
				StringFormat format = new StringFormat(StringFormatFlags.NoClip);
				//计算绘制文字所需的区域大小（根据宽度计算长度），重新创建矩形区域绘图
				sizef = g.MeasureString(text, font, PointF.Empty, format);
			}
			return sizef;

		}
		/// <summary>
		/// 写文字
		/// </summary>
		/// <param name="bmp_mmm"></param>
		/// <param name="text"></param>
		/// <param name="X"></param>
		/// <param name="Y"></param>
		/// <param name="font"></param>
		/// <param name="fontcolor"></param>
		/// <param name="backColor"></param>
		public void TextToBitmap(ref Bitmap bmp_mmm, string text, int X, int Y, Font font, Color fontcolor, Color backColor)
		{
			using (Graphics g = Graphics.FromImage(bmp_mmm))
			{
				StringFormat format = new StringFormat(StringFormatFlags.NoClip);

				//计算绘制文字所需的区域大小（根据宽度计算长度），重新创建矩形区域绘图
				SizeF sizef = g.MeasureString(text, font, PointF.Empty, format);

				int width = (int)(sizef.Width + 1);
				int height = (int)(sizef.Height + 1);
				Rectangle rect = new Rectangle(X, Y, width, height);

				//使用ClearType字体功能
				g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;
				g.FillRectangle(new SolidBrush(backColor), rect);
				g.DrawString(text, font, Brushes.Black, rect, format);
			}

		}

	}
}
