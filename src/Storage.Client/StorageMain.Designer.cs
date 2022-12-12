namespace Storage.Client;

partial class StorageMain
{
    /// <summary>
    ///  Required designer variable.
    /// </summary>
    private System.ComponentModel.IContainer components = null;

    /// <summary>
    ///  Clean up any resources being used.
    /// </summary>
    /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
    protected override void Dispose(bool disposing)
    {
        if (disposing && (components != null))
        {
            components.Dispose();
        }
        base.Dispose(disposing);
    }

    #region Windows Form Designer generated code

    /// <summary>
    ///  Required method for Designer support - do not modify
    ///  the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent()
    {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(StorageMain));
            this.StorageNotify = new System.Windows.Forms.NotifyIcon(this.components);
            this.NotifyMenus = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.minioToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.MinioMapToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ossToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.OssMapToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.退出管理ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.tabControl2 = new System.Windows.Forms.TabControl();
            this.MinIoTab = new System.Windows.Forms.TabPage();
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.MinioGroup = new System.Windows.Forms.GroupBox();
            this.MountPoint = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.ServerButton = new System.Windows.Forms.Button();
            this.SaveButton = new System.Windows.Forms.Button();
            this.MinIoVolumeLabel = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.MinIoPort = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.MinIoEndpoint = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.MinIoBucketName = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.MinIoSecretKey = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.MinIoAccessKey = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.StartDefault = new System.Windows.Forms.CheckBox();
            this.OssTab = new System.Windows.Forms.TabPage();
            this.flowLayoutPanel2 = new System.Windows.Forms.FlowLayoutPanel();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.OssVolumeLabel = new System.Windows.Forms.TextBox();
            this.label13 = new System.Windows.Forms.Label();
            this.OssServerButton = new System.Windows.Forms.Button();
            this.OssSaveConfig = new System.Windows.Forms.Button();
            this.OssBucketName = new System.Windows.Forms.TextBox();
            this.label12 = new System.Windows.Forms.Label();
            this.OssEndpoint = new System.Windows.Forms.TextBox();
            this.label11 = new System.Windows.Forms.Label();
            this.OssAccessKeySecret = new System.Windows.Forms.TextBox();
            this.label10 = new System.Windows.Forms.Label();
            this.OssAccessKeyId = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.OssMountPoint = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.OssStartDefault = new System.Windows.Forms.CheckBox();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.AddWindowServer = new System.Windows.Forms.Button();
            this.NotifyMenus.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabControl2.SuspendLayout();
            this.MinIoTab.SuspendLayout();
            this.flowLayoutPanel1.SuspendLayout();
            this.MinioGroup.SuspendLayout();
            this.OssTab.SuspendLayout();
            this.flowLayoutPanel2.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.tabPage3.SuspendLayout();
            this.SuspendLayout();
            // 
            // StorageNotify
            // 
            this.StorageNotify.Icon = ((System.Drawing.Icon)(resources.GetObject("StorageNotify.Icon")));
            this.StorageNotify.Text = "Storage管理";
            this.StorageNotify.Visible = true;
            this.StorageNotify.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.StorageNotify_MouseDoubleClick);
            // 
            // NotifyMenus
            // 
            this.NotifyMenus.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.minioToolStripMenuItem,
            this.ossToolStripMenuItem,
            this.退出管理ToolStripMenuItem});
            this.NotifyMenus.Name = "NotifyMenus";
            this.NotifyMenus.Size = new System.Drawing.Size(125, 70);
            // 
            // minioToolStripMenuItem
            // 
            this.minioToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.MinioMapToolStripMenuItem});
            this.minioToolStripMenuItem.Name = "minioToolStripMenuItem";
            this.minioToolStripMenuItem.Size = new System.Drawing.Size(124, 22);
            this.minioToolStripMenuItem.Text = "Minio";
            // 
            // MinioMapToolStripMenuItem
            // 
            this.MinioMapToolStripMenuItem.Name = "MinioMapToolStripMenuItem";
            this.MinioMapToolStripMenuItem.Size = new System.Drawing.Size(186, 22);
            this.MinioMapToolStripMenuItem.Text = "启动/关闭Minio映射";
            this.MinioMapToolStripMenuItem.Click += new System.EventHandler(this.MinIoMapToolStripMenuItem_Click);
            // 
            // ossToolStripMenuItem
            // 
            this.ossToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.OssMapToolStripMenuItem});
            this.ossToolStripMenuItem.Name = "ossToolStripMenuItem";
            this.ossToolStripMenuItem.Size = new System.Drawing.Size(124, 22);
            this.ossToolStripMenuItem.Text = "Oss";
            // 
            // OssMapToolStripMenuItem
            // 
            this.OssMapToolStripMenuItem.Name = "OssMapToolStripMenuItem";
            this.OssMapToolStripMenuItem.Size = new System.Drawing.Size(175, 22);
            this.OssMapToolStripMenuItem.Text = "启动/关闭Oss映射";
            this.OssMapToolStripMenuItem.Click += new System.EventHandler(this.OssMapToolStripMenuItem_Click);
            // 
            // 退出管理ToolStripMenuItem
            // 
            this.退出管理ToolStripMenuItem.Name = "退出管理ToolStripMenuItem";
            this.退出管理ToolStripMenuItem.Size = new System.Drawing.Size(124, 22);
            this.退出管理ToolStripMenuItem.Text = "退出服务";
            this.退出管理ToolStripMenuItem.Click += new System.EventHandler(this.ExitToolStripMenuItem_Click);
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Controls.Add(this.tabPage3);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(708, 432);
            this.tabControl1.TabIndex = 1;
            this.tabControl1.SizeChanged += new System.EventHandler(this.tabControl1_SizeChanged);
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.tabControl2);
            this.tabPage1.Location = new System.Drawing.Point(4, 26);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(700, 402);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "首页";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // tabControl2
            // 
            this.tabControl2.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControl2.Controls.Add(this.MinIoTab);
            this.tabControl2.Controls.Add(this.OssTab);
            this.tabControl2.Location = new System.Drawing.Point(0, 0);
            this.tabControl2.Name = "tabControl2";
            this.tabControl2.SelectedIndex = 0;
            this.tabControl2.Size = new System.Drawing.Size(700, 402);
            this.tabControl2.TabIndex = 2;
            // 
            // MinIoTab
            // 
            this.MinIoTab.Controls.Add(this.flowLayoutPanel1);
            this.MinIoTab.Location = new System.Drawing.Point(4, 26);
            this.MinIoTab.Name = "MinIoTab";
            this.MinIoTab.Padding = new System.Windows.Forms.Padding(3);
            this.MinIoTab.Size = new System.Drawing.Size(692, 372);
            this.MinIoTab.TabIndex = 0;
            this.MinIoTab.Text = "MinIoTab";
            this.MinIoTab.UseVisualStyleBackColor = true;
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.Controls.Add(this.MinioGroup);
            this.flowLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flowLayoutPanel1.Location = new System.Drawing.Point(3, 3);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new System.Drawing.Size(686, 366);
            this.flowLayoutPanel1.TabIndex = 0;
            // 
            // MinioGroup
            // 
            this.MinioGroup.Controls.Add(this.MountPoint);
            this.MinioGroup.Controls.Add(this.label7);
            this.MinioGroup.Controls.Add(this.ServerButton);
            this.MinioGroup.Controls.Add(this.SaveButton);
            this.MinioGroup.Controls.Add(this.MinIoVolumeLabel);
            this.MinioGroup.Controls.Add(this.label6);
            this.MinioGroup.Controls.Add(this.MinIoPort);
            this.MinioGroup.Controls.Add(this.label5);
            this.MinioGroup.Controls.Add(this.MinIoEndpoint);
            this.MinioGroup.Controls.Add(this.label4);
            this.MinioGroup.Controls.Add(this.MinIoBucketName);
            this.MinioGroup.Controls.Add(this.label3);
            this.MinioGroup.Controls.Add(this.MinIoSecretKey);
            this.MinioGroup.Controls.Add(this.label2);
            this.MinioGroup.Controls.Add(this.MinIoAccessKey);
            this.MinioGroup.Controls.Add(this.label1);
            this.MinioGroup.Controls.Add(this.StartDefault);
            this.MinioGroup.Location = new System.Drawing.Point(3, 3);
            this.MinioGroup.Name = "MinioGroup";
            this.MinioGroup.Size = new System.Drawing.Size(680, 360);
            this.MinioGroup.TabIndex = 1;
            this.MinioGroup.TabStop = false;
            this.MinioGroup.Text = "Minio启动设置";
            // 
            // MountPoint
            // 
            this.MountPoint.Location = new System.Drawing.Point(277, 37);
            this.MountPoint.Name = "MountPoint";
            this.MountPoint.Size = new System.Drawing.Size(133, 23);
            this.MountPoint.TabIndex = 16;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(203, 40);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(68, 17);
            this.label7.TabIndex = 15;
            this.label7.Text = "挂载的盘符";
            // 
            // ServerButton
            // 
            this.ServerButton.Location = new System.Drawing.Point(403, 234);
            this.ServerButton.Name = "ServerButton";
            this.ServerButton.Size = new System.Drawing.Size(103, 58);
            this.ServerButton.TabIndex = 14;
            this.ServerButton.Text = "开启服务";
            this.ServerButton.UseVisualStyleBackColor = true;
            this.ServerButton.Click += new System.EventHandler(this.ServerButton_Click);
            // 
            // SaveButton
            // 
            this.SaveButton.Location = new System.Drawing.Point(148, 234);
            this.SaveButton.Name = "SaveButton";
            this.SaveButton.Size = new System.Drawing.Size(103, 58);
            this.SaveButton.TabIndex = 13;
            this.SaveButton.Text = "保存配置";
            this.SaveButton.UseVisualStyleBackColor = true;
            this.SaveButton.Click += new System.EventHandler(this.SaveButton_Click);
            // 
            // MinIoVolumeLabel
            // 
            this.MinIoVolumeLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.MinIoVolumeLabel.Location = new System.Drawing.Point(512, 141);
            this.MinIoVolumeLabel.Name = "MinIoVolumeLabel";
            this.MinIoVolumeLabel.Size = new System.Drawing.Size(58, 23);
            this.MinIoVolumeLabel.TabIndex = 12;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(447, 147);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(68, 17);
            this.label6.TabIndex = 11;
            this.label6.Text = "挂载盘名称";
            // 
            // MinIoPort
            // 
            this.MinIoPort.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.MinIoPort.Location = new System.Drawing.Point(277, 141);
            this.MinIoPort.Name = "MinIoPort";
            this.MinIoPort.Size = new System.Drawing.Size(124, 23);
            this.MinIoPort.TabIndex = 10;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(217, 144);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(56, 17);
            this.label5.TabIndex = 9;
            this.label5.Text = "服务端口";
            // 
            // MinIoEndpoint
            // 
            this.MinIoEndpoint.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.MinIoEndpoint.Location = new System.Drawing.Point(55, 141);
            this.MinIoEndpoint.Name = "MinIoEndpoint";
            this.MinIoEndpoint.Size = new System.Drawing.Size(58, 23);
            this.MinIoEndpoint.TabIndex = 8;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(6, 147);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(56, 17);
            this.label4.TabIndex = 7;
            this.label4.Text = "服务地址";
            // 
            // MinIoBucketName
            // 
            this.MinIoBucketName.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.MinIoBucketName.Location = new System.Drawing.Point(512, 89);
            this.MinIoBucketName.Name = "MinIoBucketName";
            this.MinIoBucketName.Size = new System.Drawing.Size(58, 23);
            this.MinIoBucketName.TabIndex = 6;
            // 
            // label3
            // 
            this.label3.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(447, 92);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(59, 17);
            this.label3.TabIndex = 5;
            this.label3.Text = "Bucket桶";
            // 
            // MinIoSecretKey
            // 
            this.MinIoSecretKey.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.MinIoSecretKey.Location = new System.Drawing.Point(277, 89);
            this.MinIoSecretKey.Name = "MinIoSecretKey";
            this.MinIoSecretKey.Size = new System.Drawing.Size(124, 23);
            this.MinIoSecretKey.TabIndex = 4;
            // 
            // label2
            // 
            this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(228, 92);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(32, 17);
            this.label2.TabIndex = 3;
            this.label2.Text = "密码";
            // 
            // MinIoAccessKey
            // 
            this.MinIoAccessKey.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.MinIoAccessKey.Location = new System.Drawing.Point(55, 89);
            this.MinIoAccessKey.Name = "MinIoAccessKey";
            this.MinIoAccessKey.Size = new System.Drawing.Size(58, 23);
            this.MinIoAccessKey.TabIndex = 2;
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 92);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(32, 17);
            this.label1.TabIndex = 1;
            this.label1.Text = "账号";
            // 
            // StartDefault
            // 
            this.StartDefault.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.StartDefault.AutoSize = true;
            this.StartDefault.Location = new System.Drawing.Point(59, 40);
            this.StartDefault.Name = "StartDefault";
            this.StartDefault.Size = new System.Drawing.Size(63, 21);
            this.StartDefault.TabIndex = 0;
            this.StartDefault.Text = "自启动";
            this.StartDefault.UseVisualStyleBackColor = true;
            // 
            // OssTab
            // 
            this.OssTab.Controls.Add(this.flowLayoutPanel2);
            this.OssTab.Location = new System.Drawing.Point(4, 26);
            this.OssTab.Name = "OssTab";
            this.OssTab.Padding = new System.Windows.Forms.Padding(3);
            this.OssTab.Size = new System.Drawing.Size(692, 372);
            this.OssTab.TabIndex = 1;
            this.OssTab.Text = "OssTab";
            this.OssTab.UseVisualStyleBackColor = true;
            // 
            // flowLayoutPanel2
            // 
            this.flowLayoutPanel2.Controls.Add(this.groupBox1);
            this.flowLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flowLayoutPanel2.Location = new System.Drawing.Point(3, 3);
            this.flowLayoutPanel2.Name = "flowLayoutPanel2";
            this.flowLayoutPanel2.Size = new System.Drawing.Size(686, 366);
            this.flowLayoutPanel2.TabIndex = 0;
            // 
            // groupBox1
            // 
            this.groupBox1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.groupBox1.Controls.Add(this.OssVolumeLabel);
            this.groupBox1.Controls.Add(this.label13);
            this.groupBox1.Controls.Add(this.OssServerButton);
            this.groupBox1.Controls.Add(this.OssSaveConfig);
            this.groupBox1.Controls.Add(this.OssBucketName);
            this.groupBox1.Controls.Add(this.label12);
            this.groupBox1.Controls.Add(this.OssEndpoint);
            this.groupBox1.Controls.Add(this.label11);
            this.groupBox1.Controls.Add(this.OssAccessKeySecret);
            this.groupBox1.Controls.Add(this.label10);
            this.groupBox1.Controls.Add(this.OssAccessKeyId);
            this.groupBox1.Controls.Add(this.label9);
            this.groupBox1.Controls.Add(this.OssMountPoint);
            this.groupBox1.Controls.Add(this.label8);
            this.groupBox1.Controls.Add(this.OssStartDefault);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupBox1.Location = new System.Drawing.Point(3, 3);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(686, 366);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "groupBox1";
            // 
            // OssVolumeLabel
            // 
            this.OssVolumeLabel.Location = new System.Drawing.Point(123, 189);
            this.OssVolumeLabel.Name = "OssVolumeLabel";
            this.OssVolumeLabel.Size = new System.Drawing.Size(100, 23);
            this.OssVolumeLabel.TabIndex = 14;
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(50, 192);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(68, 17);
            this.label13.TabIndex = 13;
            this.label13.Text = "盘符名称：";
            // 
            // OssServerButton
            // 
            this.OssServerButton.Location = new System.Drawing.Point(412, 241);
            this.OssServerButton.Name = "OssServerButton";
            this.OssServerButton.Size = new System.Drawing.Size(89, 43);
            this.OssServerButton.TabIndex = 12;
            this.OssServerButton.Text = "启动服务";
            this.OssServerButton.UseVisualStyleBackColor = true;
            this.OssServerButton.Click += new System.EventHandler(this.OssStartServer_Click);
            // 
            // OssSaveConfig
            // 
            this.OssSaveConfig.Location = new System.Drawing.Point(103, 241);
            this.OssSaveConfig.Name = "OssSaveConfig";
            this.OssSaveConfig.Size = new System.Drawing.Size(91, 43);
            this.OssSaveConfig.TabIndex = 11;
            this.OssSaveConfig.Text = "保存配置";
            this.OssSaveConfig.UseVisualStyleBackColor = true;
            this.OssSaveConfig.Click += new System.EventHandler(this.OssSaveConfig_Click);
            // 
            // OssBucketName
            // 
            this.OssBucketName.Location = new System.Drawing.Point(412, 145);
            this.OssBucketName.Name = "OssBucketName";
            this.OssBucketName.Size = new System.Drawing.Size(159, 23);
            this.OssBucketName.TabIndex = 10;
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(262, 153);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(128, 17);
            this.label12.TabIndex = 9;
            this.label12.Text = "MinIoBucketName：";
            // 
            // OssEndpoint
            // 
            this.OssEndpoint.Location = new System.Drawing.Point(124, 147);
            this.OssEndpoint.Name = "OssEndpoint";
            this.OssEndpoint.Size = new System.Drawing.Size(100, 23);
            this.OssEndpoint.TabIndex = 8;
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(12, 151);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(106, 17);
            this.label11.TabIndex = 7;
            this.label11.Text = "MinIoEndpoint：";
            // 
            // OssAccessKeySecret
            // 
            this.OssAccessKeySecret.Location = new System.Drawing.Point(412, 98);
            this.OssAccessKeySecret.Name = "OssAccessKeySecret";
            this.OssAccessKeySecret.Size = new System.Drawing.Size(159, 23);
            this.OssAccessKeySecret.TabIndex = 6;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(274, 101);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(116, 17);
            this.label10.TabIndex = 5;
            this.label10.Text = "AccessKeySecret：";
            // 
            // OssAccessKeyId
            // 
            this.OssAccessKeyId.Location = new System.Drawing.Point(124, 95);
            this.OssAccessKeyId.Name = "OssAccessKeyId";
            this.OssAccessKeyId.Size = new System.Drawing.Size(102, 23);
            this.OssAccessKeyId.TabIndex = 4;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(26, 101);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(92, 17);
            this.label9.TabIndex = 3;
            this.label9.Text = "AccessKeyId：";
            // 
            // OssMountPoint
            // 
            this.OssMountPoint.Location = new System.Drawing.Point(412, 37);
            this.OssMountPoint.Name = "OssMountPoint";
            this.OssMountPoint.Size = new System.Drawing.Size(159, 23);
            this.OssMountPoint.TabIndex = 2;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(310, 43);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(80, 17);
            this.label8.TabIndex = 1;
            this.label8.Text = "挂载的盘符：";
            // 
            // OssStartDefault
            // 
            this.OssStartDefault.AutoSize = true;
            this.OssStartDefault.Location = new System.Drawing.Point(57, 43);
            this.OssStartDefault.Name = "OssStartDefault";
            this.OssStartDefault.Size = new System.Drawing.Size(63, 21);
            this.OssStartDefault.TabIndex = 0;
            this.OssStartDefault.Text = "自启动";
            this.OssStartDefault.UseVisualStyleBackColor = true;
            // 
            // tabPage2
            // 
            this.tabPage2.Location = new System.Drawing.Point(4, 26);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(700, 402);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "关于";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // tabPage3
            // 
            this.tabPage3.Controls.Add(this.AddWindowServer);
            this.tabPage3.Location = new System.Drawing.Point(4, 26);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage3.Size = new System.Drawing.Size(700, 402);
            this.tabPage3.TabIndex = 2;
            this.tabPage3.Text = "设置";
            this.tabPage3.UseVisualStyleBackColor = true;
            // 
            // AddWindowServer
            // 
            this.AddWindowServer.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.AddWindowServer.Location = new System.Drawing.Point(8, 6);
            this.AddWindowServer.Name = "AddWindowServer";
            this.AddWindowServer.Size = new System.Drawing.Size(689, 32);
            this.AddWindowServer.TabIndex = 0;
            this.AddWindowServer.Text = "注册Window服务";
            this.AddWindowServer.UseVisualStyleBackColor = true;
            this.AddWindowServer.Click += new System.EventHandler(this.AddWindowServer_Click);
            // 
            // StorageMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(708, 432);
            this.Controls.Add(this.tabControl1);
            this.Name = "StorageMain";
            this.ShowIcon = false;
            this.Text = "StorageMain";
            this.Shown += new System.EventHandler(this.StorageMain_Shown);
            this.NotifyMenus.ResumeLayout(false);
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabControl2.ResumeLayout(false);
            this.MinIoTab.ResumeLayout(false);
            this.flowLayoutPanel1.ResumeLayout(false);
            this.MinioGroup.ResumeLayout(false);
            this.MinioGroup.PerformLayout();
            this.OssTab.ResumeLayout(false);
            this.flowLayoutPanel2.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.tabPage3.ResumeLayout(false);
            this.ResumeLayout(false);

    }

    #endregion
    private NotifyIcon StorageNotify;
    private ContextMenuStrip NotifyMenus;
    private ToolStripMenuItem 退出管理ToolStripMenuItem;
    private ToolStripMenuItem minioToolStripMenuItem;
    private ToolStripMenuItem MinioMapToolStripMenuItem;
    private TabControl tabControl1;
    private TabPage tabPage1;
    private TabPage tabPage2;
    private TabPage tabPage3;
    private Button AddWindowServer;
    private TabControl tabControl2;
    private TabPage OssTab;
    private TabPage MinIoTab;
    private FlowLayoutPanel flowLayoutPanel1;
    private GroupBox MinioGroup;
    private TextBox MountPoint;
    private Label label7;
    private Button ServerButton;
    private Button SaveButton;
    private TextBox MinIoVolumeLabel;
    private Label label6;
    private TextBox MinIoPort;
    private Label label5;
    private TextBox MinIoEndpoint;
    private Label label4;
    private TextBox MinIoBucketName;
    private Label label3;
    private TextBox MinIoSecretKey;
    private Label label2;
    private TextBox MinIoAccessKey;
    private Label label1;
    private CheckBox StartDefault;
    private FlowLayoutPanel flowLayoutPanel2;
    private GroupBox groupBox1;
    private TextBox OssMountPoint;
    private Label label8;
    private CheckBox OssStartDefault;
    private TextBox OssAccessKeyId;
    private Label label9;
    private Label label10;
    private TextBox OssAccessKeySecret;
    private TextBox OssBucketName;
    private Label label12;
    private TextBox OssEndpoint;
    private Label label11;
    private Button OssServerButton;
    private Button OssSaveConfig;
    private ToolStripMenuItem ossToolStripMenuItem;
    private ToolStripMenuItem OssMapToolStripMenuItem;
    private TextBox OssVolumeLabel;
    private Label label13;
}
