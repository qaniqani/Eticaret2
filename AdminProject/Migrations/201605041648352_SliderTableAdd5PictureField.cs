namespace AdminProject.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SliderTableAdd5PictureField : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Slider", "Picture6", c => c.String());
            AddColumn("dbo.Slider", "Picture7", c => c.String());
            AddColumn("dbo.Slider", "Picture8", c => c.String());
            AddColumn("dbo.Slider", "Picture9", c => c.String());
            AddColumn("dbo.Slider", "Picture10", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Slider", "Picture10");
            DropColumn("dbo.Slider", "Picture9");
            DropColumn("dbo.Slider", "Picture8");
            DropColumn("dbo.Slider", "Picture7");
            DropColumn("dbo.Slider", "Picture6");
        }
    }
}
