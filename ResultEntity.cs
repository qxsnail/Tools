using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebHelper
{
    public class ResultEntity
    {
        /// <summary>
        /// 操作是否成功
        /// </summary>
        public bool Result { get; set; }
        /// <summary>
        /// 执行返回消息  
        /// </summary>
        public string Message { get; set; }
        /// <summary>
        /// 其他扩展消息
        /// </summary>
        public string Remark { get; set; }
    }

    public class ResultEntity<T>
    {
        /// <summary>
        /// 操作是否成功
        /// </summary>
        public bool Result { get; set; }
        /// <summary>
        /// 执行返回消息  
        /// </summary>
        public string Message { get; set; }
        /// <summary>
        /// 返回结果集
        /// </summary>
        public List<T> DataList { get; set; }
        /// <summary>
        /// 返回结果
        /// </summary>
        public T DataInfo { get; set; }
    }


}
