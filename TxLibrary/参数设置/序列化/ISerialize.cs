/**********************************************************************
 * File name:		ISerialize.cs
 * Author:			landy
 * Create date:		2020年10月22日
 * Description:		存储接口
 * History:			
 * <Date>			<Author>	<Description>
 * 2020年10月22日   landy       创建Save/Load接口
 * 2020年10月23日   landy       重命名为ISerialize，原命名为IMemory
 **********************************************************************/

namespace TxLibrary.Config.Serialize
{
    /// <summary>
    /// 参数存储接口
    /// </summary>
    public interface ISerialize<T>
    {
        /// <summary>
        /// 保存参数
        /// </summary>
        /// <param name="file"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        bool Save(string file, T value);

        /// <summary>
        /// 加载参数
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        T Load(string file);
    }
}
