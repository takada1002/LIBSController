using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LIBSController
{
	public partial class CounterBoardDebugView : Form
	{
		#region インスタンス

		/// <summary>
		/// インスタンス
		/// </summary>
		public static CounterBoardDebugView Instance = new CounterBoardDebugView();

		#endregion

		#region イベント

		/// <summary>
		/// ポート入力変化イベントパラメータ
		/// </summary>
		public class PortInputChangeEventArgs :EventArgs
		{
			public int BoardNo { get; set; }
			public int PortNo { get; set; }
			public bool Value { get; set; }

			public PortInputChangeEventArgs(int board, int port ,bool value)
			{
				this.BoardNo = board;
				this.PortNo = port;
				this.Value = value;
			}
		}

		/// <summary>
		/// ポート入力変化イベント
		/// </summary>
		public event EventHandler<PortInputChangeEventArgs> PortInputChanged;

		#endregion

		#region ポートデータ

		/// <summary>
		/// ポートデータ
		/// </summary>
		public class PortData
		{
			public int BoardNo { get; set; }
			public int PortNo { get; set; }
			public string PortName { get; set; }
			public bool IsOutput { get; set; }
			public bool Value { get; set; }
			public Button ButtonOn { get; set; }
			public Button ButtonOff { get; set; }
			public TextBox TextBox { get; set; }

			public PortData(int board, int port, bool isOut , Button btnOn, Button btnOff, TextBox tb, string name)
			{
				this.BoardNo = board;
				this.PortNo = port;
				this.PortName = name;
				this.ButtonOn = btnOn;
				this.ButtonOff = btnOff;
				this.TextBox = tb;

				SetIsOutput(isOut);

				this.ButtonOn.Click += (s,e)=> SetValue(true);
				this.ButtonOff.Click += (s, e) => SetValue(false);
			}

			public void SetIsOutput(bool isOut)
			{
				if (this.IsOutput == isOut)
					return;

				if (this.ButtonOn.InvokeRequired)
				{
					this.ButtonOn.Invoke(new Action(()=> SetIsOutput(isOut)));
					return;
				}

				this.IsOutput = isOut;

				this.ButtonOn.Enabled = !isOut;
				this.ButtonOff.Enabled = !isOut;
			}

			public void SetValue(bool value)
			{
				if(this.Value == value)
					return;
				
					if (this.ButtonOn.InvokeRequired)
				{
					this.ButtonOn.Invoke(new Action(() => SetValue(value)));
					return;
				}

				this.Value = value;
				if(value)
					TextBox.BackColor = Color.Green;
				else
					TextBox.BackColor = Color.White;
			}
		}

		/// <summary>
		/// ポートデータリスト
		/// </summary>
		public List<PortData> PortList;

		#endregion

		#region 初期化
		
		/// <summary>
		/// コンストラクタ
		/// </summary>
		public CounterBoardDebugView()
		{
			InitializeComponent();

			PortList = new List<PortData>()
			{
				new PortData(1, 1,	true,	this.btnON1_1 ,	btnOFF1_1 ,	this.tb1_1  ,"パドル出力1"),
				new PortData(1, 2,	true,	this.btnON1_2 ,	btnOFF1_2 ,	this.tb1_2	,"パドル出力2"),
				new PortData(1, 3,	true,	this.btnON1_3 ,	btnOFF1_3 ,	this.tb1_3	,"パドル出力3"),
				new PortData(1, 4,	true,	this.btnON1_4 ,	btnOFF1_4 ,	this.tb1_4	,"パドル出力4"),
				new PortData(1, 5,	true,	this.btnON1_5 ,	btnOFF1_5 ,	this.tb1_5	,"パドル出力5"),
				new PortData(1, 6,	true,	this.btnON1_6 ,	btnOFF1_6 ,	this.tb1_6	,"パドル出力6"),
				new PortData(1, 7,	true,	this.btnON1_7 ,	btnOFF1_7 ,	this.tb1_7	,"ｲﾝﾀｰﾛｯｸ"),
				new PortData(1, 8,	false,	this.btnON1_8 ,	btnOFF1_8 ,	this.tb1_8	,"緊急停止入力(引き網)"),
				new PortData(1, 9,  false,	this.btnON1_9 ,	btnOFF1_9 ,	this.tb1_9	,"ｺﾍﾞｱ起動SW入力"),
				new PortData(1, 10, true,	this.btnON1_10 ,btnOFF1_10 ,this.tb1_10	,"ｺﾝﾍﾞｱ起動LED出力"),
				new PortData(1, 11, false,	this.btnON1_11 ,btnOFF1_11 ,this.tb1_11	,"ｴﾝｺｰﾀﾞ異常"),
				new PortData(1, 12, true,	this.btnON1_12 ,btnOFF1_12 ,this.tb1_12	,"LIBS起動LED出力"),
				new PortData(1, 13, false,	this.btnON1_13 ,btnOFF1_13 ,this.tb1_13	,"ｼｽﾃﾑ起動SW入力"),
				new PortData(1, 14, true,	this.btnON1_14 ,btnOFF1_14 ,this.tb1_14	,"非常停止LED出力"),
				new PortData(1, 15, false,	this.btnON1_15 ,btnOFF1_15 ,this.tb1_15	,"非常停止SW入力"),
				new PortData(1, 16, true,	this.btnON1_16 ,btnOFF1_16 ,this.tb1_16	,"ｼｽﾃﾑ起動LED出力"),

				new PortData(2, 1,  true,   this.btnON2_1 , btnOFF2_1 , this.tb2_1  ,"外部信号:運転中"),
				new PortData(2, 2,  true,   this.btnON2_2 , btnOFF2_2 , this.tb2_2  ,"外部信号:停止中"),
				new PortData(2, 3,  true,   this.btnON2_3 , btnOFF2_3 , this.tb2_3  ,"外部信号:非常停止中"),
				new PortData(2, 4,  true,   this.btnON2_4 , btnOFF2_4 , this.tb2_4  ,"外部信号:ｲﾝﾊﾞｰﾀｰ異常"),
				new PortData(2, 5,  true,   this.btnON2_5 , btnOFF2_5 , this.tb2_5  ,"外部信号:LIBS異常"),
				new PortData(2, 6,  true,   this.btnON2_6 , btnOFF2_6 , this.tb2_6  ,""),
				new PortData(2, 7,  true,   this.btnON2_7 , btnOFF2_7 , this.tb2_7  ,""),
				new PortData(2, 8,  true,	this.btnON2_8 , btnOFF2_8 , this.tb2_8  ,"ｴﾝｺｰﾀﾞｰ異常RESET"),
				new PortData(2, 9,  true,	this.btnON2_9 , btnOFF2_9 , this.tb2_9  ,"表示灯(赤)"),
				new PortData(2, 10, true,   this.btnON2_10 ,btnOFF2_10 ,this.tb2_10 ,"表示灯(黄)"),
				new PortData(2, 11, true,	this.btnON2_11 ,btnOFF2_11 ,this.tb2_11 ,"表示灯(緑)"),
				new PortData(2, 12, true,   this.btnON2_12 ,btnOFF2_12 ,this.tb2_12 ,"2Hzパルス出力"),
				new PortData(2, 13, true,	this.btnON2_13 ,btnOFF2_13 ,this.tb2_13 ,"PC起動信号"),
				new PortData(2, 14, true,   this.btnON2_14 ,btnOFF2_14 ,this.tb2_14 ,"ｲﾝﾊﾞｰﾀｰ異常RESET"),
				new PortData(2, 15, false,  this.btnON2_15 ,btnOFF2_15 ,this.tb2_15 ,"ｲﾝﾊﾞｰﾀｰ異常信号"),
				new PortData(2, 16, false,  this.btnON2_16 ,btnOFF2_16 ,this.tb2_16 ,"LIBS　Started up feedback信号")
			};

			this.PortList.ForEach(x=>
			{
				x.ButtonOn.Click += (s,e)=>
					PortInputChanged?.Invoke(this, new PortInputChangeEventArgs(x.BoardNo, x.PortNo, true));
				x.ButtonOff.Click += (s, e) =>
					PortInputChanged?.Invoke(this, new PortInputChangeEventArgs(x.BoardNo, x.PortNo, true));
			});
		}

		#endregion

		#region 公開メソッド

		/// <summary>
		/// 入出力セット
		/// </summary>
		/// <param name="board"></param>
		/// <param name="port"></param>
		/// <param name="value"></param>
		public void SetIsOutput(int board, int port, bool value)
		{
			this.PortList
				.FirstOrDefault(x => x.BoardNo == board && x.PortNo == port)
				.SetIsOutput(value);
		}

		/// <summary>
		/// 値セット
		/// </summary>
		/// <param name="board"></param>
		/// <param name="port"></param>
		/// <param name="value"></param>
		public void SetOutputValue(int board , int port , bool value)
		{
			this.PortList
				.FirstOrDefault(x=>x.BoardNo == board && x.PortNo == port)
				.SetValue(value);
		}

		#endregion

	}
}
