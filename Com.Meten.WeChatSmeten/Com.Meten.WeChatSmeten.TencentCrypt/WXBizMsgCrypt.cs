using System;
using System.Collections;
using System.Security.Cryptography;
using System.Text;
using System.Xml;

namespace Com.Meten.WeChatSmeten.TencentCrypt
{
    public class WXBizMsgCrypt
    {
        private string m_sToken;
        private string m_sEncodingAESKey;
        private string m_sAppID;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="sToken">公众平台上，开发者设置的Token</param>
        /// <param name="sEncodingAESKey">公众平台上，开发者设置的EncodingAESKey</param>
        /// <param name="sAppID">公众帐号的appid</param>
        public WXBizMsgCrypt(string sToken, string sEncodingAESKey, string sAppID)
        {
            m_sToken = sToken;
            m_sAppID = sAppID;
            m_sEncodingAESKey = sEncodingAESKey;
        }


        /// <summary>
        /// 检验消息的真实性，并且获取解密后的明文
        /// </summary>
        /// <param name="sMsgSignature">签名串，对应URL参数的msg_signature</param>
        /// <param name="sTimeStamp">时间戳，对应URL参数的timestamp</param>
        /// <param name="sNonce">随机串，对应URL参数的nonce</param>
        /// <param name="sPostData">密文，对应POST请求的数据</param>
        /// <param name="sMsg">解密后的原文，当return返回0时有效</param>
        /// <returns>成功0，失败返回对应的错误码</returns>
        public int DecryptMsg(string sMsgSignature, string sTimeStamp, string sNonce, string sPostData, ref string sMsg)
        {
			if (m_sEncodingAESKey.Length!=43)
			{
				return (int)WXBizMsgCryptErrorCode.WXBizMsgCrypt_IllegalAesKey;
			}
            XmlDocument doc = new XmlDocument();
            XmlNode root;
            string sEncryptMsg;
            try
            {
                doc.LoadXml(sPostData);
                root = doc.FirstChild;
                sEncryptMsg = root["Encrypt"].InnerText;
            }
            catch (Exception)
            {
                return (int)WXBizMsgCryptErrorCode.WXBizMsgCrypt_ParseXml_Error;
            }
            //verify signature
            int ret = 0;
            ret = VerifySignature(m_sToken, sTimeStamp, sNonce, sEncryptMsg, sMsgSignature);
            if (ret != 0)
                return ret;
            //decrypt
            string cpid = "";
            try
            {
                sMsg = Cryptography.AES_decrypt(sEncryptMsg, m_sEncodingAESKey, ref cpid);
            }
            catch (FormatException)
            {
                return (int)WXBizMsgCryptErrorCode.WXBizMsgCrypt_DecodeBase64_Error;
            }
            catch (Exception)
            {
                return (int)WXBizMsgCryptErrorCode.WXBizMsgCrypt_DecryptAES_Error;
            }
            if (cpid != m_sAppID)
                return (int)WXBizMsgCryptErrorCode.WXBizMsgCrypt_ValidateAppid_Error;
            return 0;
        }

        /// <summary>
        ///将企业号回复用户的消息加密打包 
        /// </summary>
        /// <param name="sReplyMsg">企业号待回复用户的消息，xml格式的字符串</param>
        /// <param name="sTimeStamp">时间戳，可以自己生成，也可以用URL参数的timestamp</param>
        /// <param name="sNonce">随机串，可以自己生成，也可以用URL参数的nonce</param>
        /// <param name="sEncryptMsg">加密后的可以直接回复用户的密文，包括msg_signature, timestamp, nonce, encrypt的xml格式的字符串,当return返回0时有效</param>
        /// <returns>成功0，失败返回对应的错误码</returns>
        public int EncryptMsg(string sReplyMsg, string sTimeStamp, string sNonce, ref string sEncryptMsg)
        {
			if (m_sEncodingAESKey.Length!=43)
			{
				return (int)WXBizMsgCryptErrorCode.WXBizMsgCrypt_IllegalAesKey;
			}
            string raw = "";
            try
            {
                raw = Cryptography.AES_encrypt(sReplyMsg, m_sEncodingAESKey, m_sAppID);
            }
            catch (Exception)
            {
                return (int)WXBizMsgCryptErrorCode.WXBizMsgCrypt_EncryptAES_Error;
            }
            string MsgSigature = "";
            int ret = 0;
            ret = GenarateSinature(m_sToken, sTimeStamp, sNonce, raw, ref MsgSigature);
            if (0 != ret)
                return ret;
            sEncryptMsg = "";

            string EncryptLabelHead = "<Encrypt><![CDATA[";
            string EncryptLabelTail = "]]></Encrypt>";
            string MsgSigLabelHead = "<MsgSignature><![CDATA[";
            string MsgSigLabelTail = "]]></MsgSignature>";
            string TimeStampLabelHead = "<TimeStamp><![CDATA[";
            string TimeStampLabelTail = "]]></TimeStamp>";
            string NonceLabelHead = "<Nonce><![CDATA[";
            string NonceLabelTail = "]]></Nonce>";
            sEncryptMsg = sEncryptMsg + "<xml>" + EncryptLabelHead + raw + EncryptLabelTail;
            sEncryptMsg = sEncryptMsg + MsgSigLabelHead + MsgSigature + MsgSigLabelTail;
            sEncryptMsg = sEncryptMsg + TimeStampLabelHead + sTimeStamp + TimeStampLabelTail;
            sEncryptMsg = sEncryptMsg + NonceLabelHead + sNonce + NonceLabelTail;
            sEncryptMsg += "</xml>";
            return 0;
        }

        public class DictionarySort : System.Collections.IComparer
        {
            public int Compare(object oLeft, object oRight)
            {
                string sLeft = oLeft as string;
                string sRight = oRight as string;
                int iLeftLength = sLeft.Length;
                int iRightLength = sRight.Length;
                int index = 0;
                while (index < iLeftLength && index < iRightLength)
                {
                    if (sLeft[index] < sRight[index])
                        return -1;
                    else if (sLeft[index] > sRight[index])
                        return 1;
                    else
                        index++;
                }
                return iLeftLength - iRightLength;

            }
        }
        //Verify Signature
        private static int VerifySignature(string sToken, string sTimeStamp, string sNonce, string sMsgEncrypt, string sSigture)
        {
            string hash = "";
            int ret = 0;
            ret = GenarateSinature(sToken, sTimeStamp, sNonce, sMsgEncrypt, ref hash);
            if (ret != 0)
                return ret;
            //System.Console.WriteLine(hash);
            if (hash == sSigture)
                return 0;
            else
            {
                return (int)WXBizMsgCryptErrorCode.WXBizMsgCrypt_ValidateSignature_Error;
            }
        }

        public static int GenarateSinature(string sToken, string sTimeStamp, string sNonce, string sMsgEncrypt ,ref string sMsgSignature)
        {
            ArrayList AL = new ArrayList();
            AL.Add(sToken);
            AL.Add(sTimeStamp);
            AL.Add(sNonce);
            AL.Add(sMsgEncrypt);
            AL.Sort(new DictionarySort());
            string raw = "";
            for (int i = 0; i < AL.Count; ++i)
            {
                raw += AL[i];
            }

            SHA1 sha;
            ASCIIEncoding enc;
            string hash = "";
            try
            {
                sha = new SHA1CryptoServiceProvider();
                enc = new ASCIIEncoding();
                byte[] dataToHash = enc.GetBytes(raw);
                byte[] dataHashed = sha.ComputeHash(dataToHash);
                hash = BitConverter.ToString(dataHashed).Replace("-", "");
                hash = hash.ToLower();
            }
            catch (Exception)
            {
                return (int)WXBizMsgCryptErrorCode.WXBizMsgCrypt_ComputeSignature_Error;
            }
            sMsgSignature = hash;
            return 0;
        }
    }
}
