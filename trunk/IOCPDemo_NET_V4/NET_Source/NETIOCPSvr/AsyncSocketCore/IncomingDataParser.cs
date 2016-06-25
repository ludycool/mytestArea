using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AsyncSocketServer
{
    /// <summary>
    /// 协议解析器，用来解析客户端接收到的命令
    /// </summary>
    public class IncomingDataParser
    {
        private string m_header;
        /// <summary>
        /// 头
        /// </summary>
        public string Header { get { return m_header; } }
        private string m_command;
        /// <summary>
        /// 命令
        /// </summary>
        public string Command { get { return m_command; } }
        private List<string> m_names;
        /// <summary>
        /// 参数列表
        /// </summary>
        public List<string> Names { get { return m_names; } }
        private List<string> m_values;
        /// <summary>
        /// 值列表
        /// </summary>
        public List<string> Values { get { return m_values; } }

        public IncomingDataParser()
        {
            m_names = new List<string>();
            m_values = new List<string>();
        }
        /// <summary>
        /// 解析
        /// </summary>
        /// <param name="protocolText"></param>
        /// <returns></returns>
        public bool DecodeProtocolText(string protocolText)
        {
            m_header = "";
            m_names.Clear();
            m_values.Clear();
            int speIndex = protocolText.IndexOf(ProtocolKey.ReturnWrap);
            if (speIndex < 0)
            {
                return false;
            }
            else
            {
                string[] tmpNameValues = protocolText.Split(new string[] { ProtocolKey.ReturnWrap }, StringSplitOptions.RemoveEmptyEntries);
                if (tmpNameValues.Length < 2) //每次命令至少包括两行
                    return false;
                for (int i = 0; i < tmpNameValues.Length; i++)
                {
                    string[] tmpStr = tmpNameValues[i].Split(new string[] { ProtocolKey.EqualSign }, StringSplitOptions.None);
                    if (tmpStr.Length > 1) //存在等号
                    {
                        if (tmpStr.Length > 2) //超过两个等号，返回失败
                            return false;
                        if (tmpStr[0].Equals(ProtocolKey.Command, StringComparison.CurrentCultureIgnoreCase))
                        {
                            m_command = tmpStr[1];
                        }
                        else
                        {
                            m_names.Add(tmpStr[0].ToLower());
                            m_values.Add(tmpStr[1]);
                        }
                    }
                }
                return true;
            }
        }

        /// <summary>
        /// 获取值
        /// </summary>
        /// <param name="protocolKey"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public bool GetValue(string protocolKey, ref string value)
        {
            int index = m_names.IndexOf(protocolKey.ToLower());
            if (index > -1)
            {
                value = m_values[index];
                return true;
            }
            else
                return false;
        }
        /// <summary>
        /// 获取值 列表
        /// </summary>
        /// <param name="protocolKey"></param>
        /// <returns></returns>
        public List<string> GetValue(string protocolKey)
        {
            List<string> result = new List<string>();
            for (int i = 0; i < m_names.Count; i++)
            {
                if (protocolKey.Equals(m_names[i], StringComparison.CurrentCultureIgnoreCase))
                    result.Add(m_values[i]);
            }
            return result;
        }

        public bool GetValue(string protocolKey, ref short value)
        {
            int index = m_names.IndexOf(protocolKey.ToLower());
            if (index > -1)
            {
                return short.TryParse(m_values[index], out value);
            }
            else
                return false;
        }

        public bool GetValue(string protocolKey, ref int value)
        {
            int index = m_names.IndexOf(protocolKey.ToLower());
            if (index > -1)
                return int.TryParse(m_values[index], out value);
            else
                return false;
        }

        public bool GetValue(string protocolKey, ref long value)
        {
            int index = m_names.IndexOf(protocolKey.ToLower());
            if (index > -1)
                return long.TryParse(m_values[index], out value);
            else
                return false;
        }

        public bool GetValue(string protocolKey, ref Single value)
        {
            int index = m_names.IndexOf(protocolKey.ToLower());
            if (index > -1)
                return Single.TryParse(m_values[index], out value);
            else
                return false;
        }

        public bool GetValue(string protocolKey, ref Double value)
        {
            int index = m_names.IndexOf(protocolKey.ToLower());
            if (index > -1)
                return Double.TryParse(m_values[index], out value);
            else
                return false;
        }
    }
}