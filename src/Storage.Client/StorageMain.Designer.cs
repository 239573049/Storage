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
            this.BlazorWebView = new Microsoft.AspNetCore.Components.WebView.WindowsForms.BlazorWebView();
            this.StorageNotify = new System.Windows.Forms.NotifyIcon(this.components);
            this.NotifyMenus = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.退出管理ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.NotifyMenus.SuspendLayout();
            this.SuspendLayout();
            // 
            // BlazorWebView
            // 
            this.BlazorWebView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.BlazorWebView.Location = new System.Drawing.Point(0, 0);
            this.BlazorWebView.Name = "BlazorWebView";
            this.BlazorWebView.Size = new System.Drawing.Size(800, 450);
            this.BlazorWebView.TabIndex = 0;
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
            this.退出管理ToolStripMenuItem});
            this.NotifyMenus.Name = "NotifyMenus";
            this.NotifyMenus.Size = new System.Drawing.Size(125, 26);
            // 
            // 退出管理ToolStripMenuItem
            // 
            this.退出管理ToolStripMenuItem.Name = "退出管理ToolStripMenuItem";
            this.退出管理ToolStripMenuItem.Size = new System.Drawing.Size(124, 22);
            this.退出管理ToolStripMenuItem.Text = "退出管理";
            this.退出管理ToolStripMenuItem.Click += new System.EventHandler(this.ExitToolStripMenuItem_Click);
            // 
            // StorageMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.BlazorWebView);
            this.Name = "StorageMain";
            this.ShowIcon = false;
            this.Text = "StorageMain";
            this.NotifyMenus.ResumeLayout(false);
            this.ResumeLayout(false);

    }

    #endregion

    private Microsoft.AspNetCore.Components.WebView.WindowsForms.BlazorWebView BlazorWebView;
    private NotifyIcon StorageNotify;
    private ContextMenuStrip NotifyMenus;
    private ToolStripMenuItem 退出管理ToolStripMenuItem;
}
