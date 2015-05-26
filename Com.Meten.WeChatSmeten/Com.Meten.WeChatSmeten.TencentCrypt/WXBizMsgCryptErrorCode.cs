namespace Com.Meten.WeChatSmeten.TencentCrypt
{
    public enum WXBizMsgCryptErrorCode
    {
        /// <summary>
        /// 成功
        /// </summary>
        WXBizMsgCrypt_OK = 0,

        /// <summary>
        /// -40001 ： 签名验证错误
        /// </summary>
        WXBizMsgCrypt_ValidateSignature_Error = -40001,

        /// <summary>
        /// -40002 :  xml解析失败
        /// </summary>
        WXBizMsgCrypt_ParseXml_Error = -40002,

        /// <summary>
        /// -40003 :  sha加密生成签名失败
        /// </summary>
        WXBizMsgCrypt_ComputeSignature_Error = -40003,

        /// <summary>
        /// -40004 :  AESKey 非法
        /// </summary>
        WXBizMsgCrypt_IllegalAesKey = -40004,

        /// <summary>
        /// -40005 :  appid 校验错误
        /// </summary>
        WXBizMsgCrypt_ValidateAppid_Error = -40005,

        /// <summary>
        /// -40006 :  AES 加密失败
        /// </summary>
        WXBizMsgCrypt_EncryptAES_Error = -40006,

        /// <summary>
        /// -40007 ： AES 解密失败
        /// </summary>
        WXBizMsgCrypt_DecryptAES_Error = -40007,

        /// <summary>
        /// -40008 ： 解密后得到的buffer非法
        /// </summary>
        WXBizMsgCrypt_IllegalBuffer = -40008,

        /// <summary>
        /// -40009 :  base64加密异常
        /// </summary>
        WXBizMsgCrypt_EncodeBase64_Error = -40009,

        /// <summary>
        /// -40010 :  base64解密异常
        /// </summary>
        WXBizMsgCrypt_DecodeBase64_Error = -40010
    };
}