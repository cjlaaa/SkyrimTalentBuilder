using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Collections; 

namespace SkyrimTalentsBuilder
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            ReadTalentData();
            FilliTalent();
            InitializeComponent();
            TabPages();
            Buttons();
        }

        //初始赋值 全局变量
        int[] iTalent = new int[182];//所有天赋点数的集合
        int iLevel = 1;
        string[] TalentTitle = new string[182];//天赋名称列表
        string[] TalentDescription = new string[182];//天赋描述列表
        string[] sTalentAddNum = new string[182];//天赋每级增长属性列表
        string[] sMaxTalentLevel = new string[182];//天赋最大等级列表
        string[] sNeedSkillLevel = new string[182];//当前需要技能等级列表
       
        public void ReadTalentData() //读取数据
        {
            string sPath = "TalentData.txt";
            string[] TalentDataLine = System.IO.File.ReadAllLines(sPath, Encoding.Default);//读取文本文件所有行存入数组TalentDataLine
            string[,] TalentData = new string[182, 5];//建立一个可以装下所有数据的二维数组
            for (int i = 0; i < 10; i++)//将每行数据分别存入二维数组。
            {
                TalentDataLine[i] = TalentDataLine[i].Replace(" ", "");//将当前数据行空格去掉
                string[] Words;//建立当前数据行分割后保存的数组Words
                char[] sChar = { '	' };//分割符(制表符)
                Words = TalentDataLine[i].Split(sChar);//将当前数据行分割并分别存入数组Words
                for (int a = 0; a < 5; a++)//分别将Words的内容保存到二维数组
                {
                    TalentData[i, a] = Words[a];
                }
            }

            for (int i = 0; i < 10;i++ )
            {
                TalentTitle[i] = TalentData[i, 0];
                TalentDescription[i] = TalentData[i, 1];
                sTalentAddNum[i] = TalentData[i, 2];
                sMaxTalentLevel[i] = TalentData[i, 3];
                sNeedSkillLevel[i] = TalentData[i, 4];
            }
        }

        public void Buttons()
        {
            for (int i = 1; i <=8; i++)//为每个选项卡Text赋值
            {
                //find返回满足条件的数组，[0]取第一个
                ((Button)this.Controls.Find("button" + i, true)[0]).Text = ButtonText(i);
            }
        }

        public void TabPages()//读取选项卡名称信息
        {
            string Path = "TabPageName.txt";
            string[] TabPageNames = System.IO.File.ReadAllLines(@Path, Encoding.Default);
            for (int i = 1; i <= 18; i++)//为每个选项卡Text赋值
            {
                //find返回满足条件的数组，[0]取第一个
                ((TabPage)this.Controls.Find("tabPage"+i , true)[0]).Text = TabPageNames[i];
            }
        }

        public string ButtonText(int iCurrentTalentNum)//读取按钮名称信息
        {
            string sButtonText;
            int iTalentTitleNum=iCurrentTalentNum;
            sButtonText = TalentTitle[iTalentTitleNum] + "\n" + iTalent[iCurrentTalentNum] + "/" + sMaxTalentLevel[iCurrentTalentNum];
            return sButtonText;
        }
        
        public string TalentDescriptions(int iCurrentTalentNum)//读取并生成天赋描述信息
        {
            string sTalentDescription;
            int iTalentTitleNum = iCurrentTalentNum;
            int iTalentLevel = iTalent[iCurrentTalentNum] * Convert.ToInt32(sTalentAddNum[iCurrentTalentNum]);//当前天赋描述中的属性值
            int iTalentLevelNext = (iTalent[iCurrentTalentNum] + 1) * Convert.ToInt32(sTalentAddNum[iCurrentTalentNum]);//下一天赋描述中的属性值
            int TempNeedSkillLevel = iTalent[iCurrentTalentNum] * Convert.ToInt32(sNeedSkillLevel[iCurrentTalentNum]) - Convert.ToInt32(sNeedSkillLevel[iCurrentTalentNum]);
            string sNeedSkillLevelDescription = "(需要技能点数" + TempNeedSkillLevel.ToString() + ")";
            string sNextNeedSkillLevelDescription = "(需要技能点数" + (TempNeedSkillLevel + Convert.ToInt32(sNeedSkillLevel[iCurrentTalentNum])) + ")";
            if (sMaxTalentLevel[iCurrentTalentNum]=="1")
            {
                if (iTalent[iCurrentTalentNum] == 0)//判断当前天赋点是否为0，是则不显示当前属性
                {
                    sTalentDescription = TalentTitle[iTalentTitleNum] +
                        "\n\n当前天赋等级" + iTalent[iCurrentTalentNum] +
                        "\n\n下一天赋等级" + (iTalent[iCurrentTalentNum] + 1) + "(需要技能点数"+Convert.ToInt32(sNeedSkillLevel[iCurrentTalentNum])+")"+
                        "\n" + TalentDescription[iTalentTitleNum] ;
                }
                else//如果当前天赋为最大点数，则不显示下一等级属性描述
                {
                    sTalentDescription = TalentTitle[iTalentTitleNum] +
                        "\n\n当前天赋等级" + iTalent[iCurrentTalentNum] + "(需要技能点数" + Convert.ToInt32(sNeedSkillLevel[iCurrentTalentNum]) + ")" +
                        "\n" + TalentDescription[iTalentTitleNum] ;
                }
            }
            else
            {
                if (iTalent[iCurrentTalentNum] == 0)//判断当前天赋点是否为0，是则不显示当前属性
                {
                    sTalentDescription = TalentTitle[iTalentTitleNum] + 
                        "\n\n当前天赋等级" + iTalent[iCurrentTalentNum]  +
                        "\n\n下一天赋等级" + (iTalent[iCurrentTalentNum] + 1) + "(需要技能点数0)"+
                        "\n" + TalentDescription[iTalentTitleNum] + iTalentLevelNext;
                }
                else
                {
                    if (iTalent[iCurrentTalentNum] == Convert.ToInt32(sMaxTalentLevel[iCurrentTalentNum]))
                    {
                        sTalentDescription = TalentTitle[iTalentTitleNum] + 
                            "\n\n当前天赋等级" + iTalent[iCurrentTalentNum] +sNeedSkillLevelDescription+ 
                            "\n" + TalentDescription[iTalentTitleNum] + iTalentLevel;
                    }
                    //如果当前天赋为最大点数，则不显示下一等级属性描述
                    else 
                    {
                        sTalentDescription = TalentTitle[iTalentTitleNum] + 
                            "\n\n当前天赋等级" + iTalent[iCurrentTalentNum] + sNeedSkillLevelDescription + 
                            "\n" + TalentDescription[iTalentTitleNum] + iTalentLevel + 
                            "\n\n下一天赋等级" + (iTalent[iCurrentTalentNum] + 1) + sNextNeedSkillLevelDescription+
                            "\n" +TalentDescription[iTalentTitleNum] + iTalentLevelNext; 
                    }
                }
            }            
            sTalentDescription = sTalentDescription+"\n\n左键单击增加点数。\n右键单击减少点数。";
            return sTalentDescription;
        }

        public void FilliTalent()
        {
            for (int iTalentNum=0; iTalentNum <182; iTalentNum++)
            {
                iTalent[iTalentNum] = 0;
            }
        }

        public void ClickLevelChange(bool bMouseButton)//点击时修改"需要等级"的方法
        {
            if (bMouseButton)
            {
                iLevel++;
                this.textBoxLevel.Text = iLevel.ToString();
            }
            else
            {
                iLevel--;
                this.textBoxLevel.Text = iLevel.ToString();
            }
        }

        public bool CheckMaxLevel(int iCurrentTalentNum) //检查天赋数是否已满
        {
            bool bIsMaxLevel=true,bNotMaxLevel=false;
            if (iTalent[iCurrentTalentNum] < Convert.ToInt32(sMaxTalentLevel[iCurrentTalentNum])) 
                return bNotMaxLevel;//当前天赋点数与当前天赋最大点数比较
            else return bIsMaxLevel;
        }

        private void button1_Click(object sender, EventArgs e)//左键点击事件
        {
            if (CheckMaxLevel(1)) return;
            ClickLevelChange(true);
            iTalent[1]++;            
            this.button1.Text = ButtonText(1);
        }

        private void button1_MouseUp(object sender, MouseEventArgs e)//右键点击事件
        {
            if (e.Button == MouseButtons.Right)
            {
                if (iTalent[1] < 1) return;
                ClickLevelChange(false);
                iTalent[1]--;
                this.button1.Text = ButtonText(1);
            }
        }

        private void button1_MouseMove(object sender, MouseEventArgs e)//浮动描述
        {
            this.toolTipTalent1.SetToolTip(this.button1, TalentDescriptions(1));
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (CheckMaxLevel(2)) return;
            ClickLevelChange(true);
            iTalent[2]++;
            this.button2.Text = ButtonText(2);
        }

        private void button2_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                if (iTalent[2] < 1) return;
                ClickLevelChange(false);
                iTalent[2]--;
                this.button2.Text = ButtonText(2);
            }
        }

        private void button2_MouseMove(object sender, MouseEventArgs e)
        {
            this.toolTipTalent2.SetToolTip(this.button2, TalentDescriptions(2));
        }
    }
}
