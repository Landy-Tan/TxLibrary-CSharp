/**********************************************************************
 * File name:		IPage.cs
 * Author:			txl
 * Create date:		
 * Description:		分页纪磊
 * History:			
 * <Date>			<Author>	<Description>
 * 2020年9月10日    txl         迁移到WPF，测试完成
 **********************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace TxLibrary.Config.Page
{
    /// <summary>
    /// 分页接口
    /// </summary>
    public abstract class IPage : UserControl
    {
        /// <summary>
        /// 添加控件
        /// </summary>
        /// <param name="name"></param>
        /// <param name="ctl"></param>
        /// <returns></returns>
        public abstract bool AddControl(string name, FrameworkElement ctl);
    }
}
