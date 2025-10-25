using System;
using System.Diagnostics;
using System.IO;
using System.Windows;

namespace LangrisserTools.Main
{
    /// <summary>
    /// 兰古利萨工具集主界面
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 三本团时间计算工具按钮点击事件
        /// </summary>
        private void TimeCalculatorButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // 获取 TimeCalculatorForGuild 项目的可执行文件路径（先尝试在解决方案根目录下递归查找）
                var solutionRoot = GetSolutionRoot();
                string executablePath = null;

                if (!string.IsNullOrEmpty(solutionRoot))
                {
                    try
                    {
                        var matches = Directory.GetFiles(solutionRoot, "TimeCalculatorForGuild.exe", SearchOption.AllDirectories);
                        if (matches.Length > 0)
                        {
                            executablePath = matches[0];
                        }
                    }
                    catch
                    {
                        // 忽略搜索期间的任何权限等异常，后面会尝试其它方式
                    }
                }

                // 如果在解决方案目录未找到，则尝试在当前工作目录及其子目录中查找（作为备用）
                if (string.IsNullOrEmpty(executablePath))
                {
                    try
                    {
                        var matches = Directory.GetFiles(Directory.GetCurrentDirectory(), "TimeCalculatorForGuild.exe", SearchOption.AllDirectories);
                        if (matches.Length > 0)
                        {
                            executablePath = matches[0];
                        }
                    }
                    catch
                    {
                        // 忽略
                    }
                }

                // 仍未找到时，构造常见的默认路径作为最后尝试（例如 Debug 输出路径）
                if (string.IsNullOrEmpty(executablePath) && !string.IsNullOrEmpty(solutionRoot))
                {
                    var defaultPath = Path.Combine(solutionRoot, "TimeCalculatorForGuild", "bin", "Debug", "net10.0-windows", "TimeCalculatorForGuild.exe");
                    if (File.Exists(defaultPath))
                    {
                        executablePath = defaultPath;
                    }
                }

                if (!string.IsNullOrEmpty(executablePath) && File.Exists(executablePath))
                {
                    // 启动 TimeCalculatorForGuild 应用
                    Process.Start(new ProcessStartInfo
                    {
                        FileName = executablePath,
                        UseShellExecute = true
                    });
                }
                else
                {
                    MessageBox.Show($"找不到三本团时间计算工具的可执行文件：\n{executablePath ?? "(未找到)"}\n\n请确保项目已正确编译或在解决方案目录中存在 TimeCalculatorForGuild.exe。", 
                                  "错误", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"启动三本团时间计算工具时发生错误：\n{ex}", 
                              "错误", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        /// <summary>
        /// 获取解决方案根目录
        /// </summary>
        /// <returns>解决方案根目录路径</returns>
        private string GetSolutionRoot()
        {
            var currentDirectory = Directory.GetCurrentDirectory();
            var directory = new DirectoryInfo(currentDirectory);

            // 向上查找包含 .sln 文件的目录（修复原来错误使用的 .slnx）
            while (directory != null)
            {
                try
                {
                    if (Directory.GetFiles(directory.FullName, "*.sln").Length > 0)
                    {
                        return directory.FullName;
                    }
                }
                catch
                {
                    // 忽略访问异常，继续向上查找
                }

                directory = directory.Parent;
            }

            // 如果找不到解决方案文件，返回当前目录
            return currentDirectory;
        }
    }
}