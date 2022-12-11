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
            this.退出管理ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.minioToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.MinioMapToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.MinioGroup = new System.Windows.Forms.GroupBox();
            this.MountPoint = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.ServerButton = new System.Windows.Forms.Button();
            this.SaveButton = new System.Windows.Forms.Button();
            this.VolumeLabel = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.Port = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.Endpoint = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.BucketName = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.SecretKey = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.AccessKey = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.StartDefault = new System.Windows.Forms.CheckBox();
            this.MapList = new System.Windows.Forms.ComboBox();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.AddWindowServer = new System.Windows.Forms.Button();
            this.NotifyMenus.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.MinioGroup.SuspendLayout();
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
            this.退出管理ToolStripMenuItem});
            this.NotifyMenus.Name = "NotifyMenus";
            this.NotifyMenus.Size = new System.Drawing.Size(181, 70);
            // 
            // 退出管理ToolStripMenuItem
            // 
            this.退出管理ToolStripMenuItem.Name = "退出管理ToolStripMenuItem";
            this.退出管理ToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.退出管理ToolStripMenuItem.Text = "退出服务";
            this.退出管理ToolStripMenuItem.Click += new System.EventHandler(this.ExitToolStripMenuItem_Click);
            // 
            // minioToolStripMenuItem
            // 
            this.minioToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.MinioMapToolStripMenuItem});
            this.minioToolStripMenuItem.Name = "minioToolStripMenuItem";
            this.minioToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.minioToolStripMenuItem.Text = "Minio";
            // 
            // 启动关闭Minio映射ToolStripMenuItem
            // 
            this.MinioMapToolStripMenuItem.Name = "启动关闭Minio映射ToolStripMenuItem";
            this.MinioMapToolStripMenuItem.Size = new System.Drawing.Size(186, 22);
            this.MinioMapToolStripMenuItem.Text = "启动/关闭Minio映射";
            this.MinioMapToolStripMenuItem.Click += new System.EventHandler(this.MinioMapToolStripMenuItem_Click);
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
            this.tabPage1.Controls.Add(this.MinioGroup);
            this.tabPage1.Controls.Add(this.MapList);
            this.tabPage1.Location = new System.Drawing.Point(4, 26);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(700, 402);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "首页";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // MinioGroup
            // 
            this.MinioGroup.Controls.Add(this.MountPoint);
            this.MinioGroup.Controls.Add(this.label7);
            this.MinioGroup.Controls.Add(this.ServerButton);
            this.MinioGroup.Controls.Add(this.SaveButton);
            this.MinioGroup.Controls.Add(this.VolumeLabel);
            this.MinioGroup.Controls.Add(this.label6);
            this.MinioGroup.Controls.Add(this.Port);
            this.MinioGroup.Controls.Add(this.label5);
            this.MinioGroup.Controls.Add(this.Endpoint);
            this.MinioGroup.Controls.Add(this.label4);
            this.MinioGroup.Controls.Add(this.BucketName);
            this.MinioGroup.Controls.Add(this.label3);
            this.MinioGroup.Controls.Add(this.SecretKey);
            this.MinioGroup.Controls.Add(this.label2);
            this.MinioGroup.Controls.Add(this.AccessKey);
            this.MinioGroup.Controls.Add(this.label1);
            this.MinioGroup.Controls.Add(this.StartDefault);
            this.MinioGroup.Location = new System.Drawing.Point(5, 42);
            this.MinioGroup.Name = "MinioGroup";
            this.MinioGroup.Size = new System.Drawing.Size(689, 364);
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
            // VolumeLabel
            // 
            this.VolumeLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.VolumeLabel.Location = new System.Drawing.Point(512, 141);
            this.VolumeLabel.Name = "VolumeLabel";
            this.VolumeLabel.Size = new System.Drawing.Size(67, 23);
            this.VolumeLabel.TabIndex = 12;
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
            // Port
            // 
            this.Port.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.Port.Location = new System.Drawing.Point(277, 141);
            this.Port.Name = "Port";
            this.Port.Size = new System.Drawing.Size(133, 23);
            this.Port.TabIndex = 10;
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
            // Endpoint
            // 
            this.Endpoint.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.Endpoint.Location = new System.Drawing.Point(55, 141);
            this.Endpoint.Name = "Endpoint";
            this.Endpoint.Size = new System.Drawing.Size(67, 23);
            this.Endpoint.TabIndex = 8;
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
            // BucketName
            // 
            this.BucketName.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.BucketName.Location = new System.Drawing.Point(512, 89);
            this.BucketName.Name = "BucketName";
            this.BucketName.Size = new System.Drawing.Size(67, 23);
            this.BucketName.TabIndex = 6;
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
            // SecretKey
            // 
            this.SecretKey.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.SecretKey.Location = new System.Drawing.Point(277, 89);
            this.SecretKey.Name = "SecretKey";
            this.SecretKey.Size = new System.Drawing.Size(133, 23);
            this.SecretKey.TabIndex = 4;
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
            // AccessKey
            // 
            this.AccessKey.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.AccessKey.Location = new System.Drawing.Point(55, 89);
            this.AccessKey.Name = "AccessKey";
            this.AccessKey.Size = new System.Drawing.Size(67, 23);
            this.AccessKey.TabIndex = 2;
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
            // MapList
            // 
            this.MapList.FormattingEnabled = true;
            this.MapList.Location = new System.Drawing.Point(8, 6);
            this.MapList.Name = "MapList";
            this.MapList.Size = new System.Drawing.Size(121, 25);
            this.MapList.TabIndex = 0;
            this.MapList.SelectedIndexChanged += new System.EventHandler(this.MapList_SelectedIndexChanged);
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
            this.MinioGroup.ResumeLayout(false);
            this.MinioGroup.PerformLayout();
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
    private ComboBox MapList;
    private GroupBox MinioGroup;
    private CheckBox StartDefault;
    private Button ServerButton;
    private Button SaveButton;
    private TextBox VolumeLabel;
    private Label label6;
    private TextBox Port;
    private Label label5;
    private TextBox Endpoint;
    private Label label4;
    private TextBox BucketName;
    private Label label3;
    private TextBox SecretKey;
    private Label label2;
    private TextBox AccessKey;
    private Label label1;
    private TextBox MountPoint;
    private Label label7;
    private Button AddWindowServer;
}
