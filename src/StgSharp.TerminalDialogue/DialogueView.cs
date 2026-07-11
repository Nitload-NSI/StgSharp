

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Terminal.Gui;

namespace StgSharpTerminalDialogue
{
    internal class DialogueView:Window
    {
        TextView tittle = new TextView()
        {
            Text = """"
                
                ███████╗████████╗ ██████╗ ███████╗██╗  ██╗ █████╗ ██████╗ ██████╗ 
                ██╔════╝╚══██╔══╝██╔════╝ ██╔════╝██║  ██║██╔══██╗██╔══██╗██╔══██╗
                ███████╗   ██║   ██║  ███╗███████╗███████║███████║██████╔╝██████╔╝
                ╚════██║   ██║   ██║   ██║╚════██║██╔══██║██╔══██║██╔══██╗██╔═══╝ 
                ███████║   ██║   ╚██████╔╝███████║██║  ██║██║  ██║██║  ██║██║     
                ╚══════╝   ╚═╝    ╚═════╝ ╚══════╝╚═╝  ╚═╝╚═╝  ╚═╝╚═╝  ╚═╝╚═╝     
                
                """",
            //generate from site: https://www.asciiart.eu/text-to-ascii-art
            //font: ANSI shadow
            TextAlignment = Alignment.Start,
            Width = 8 * 9,
            Height = 1 * 8,
            X = 0,
            Y = 0,
            ColorScheme = common
        };

        static readonly Terminal.Gui.Attribute 
            commonColor = Application.Driver!.MakeColor(
                new Color(92, 180, 225), Color.Cyan),
            errorColor = Application.Driver!.MakeColor(
                Color.White,  Color.BrightRed);

        static ColorScheme common = new ColorScheme(){
            Focus = commonColor,
            Normal = commonColor
        },
        error = new ColorScheme()
        {
            Focus = errorColor,
            Normal = errorColor
        };

        public static DialogueView Single
        {
            get => new DialogueView();
        }

        private DialogueView()
        {
            Init();
        }

        private void Init()
        {
            ColorScheme = common;

            this.Width = Dim.Fill(0);
            this.Height = Dim.Fill(0);
            this.X = 0;
            this.Y = 0;
            this.Title = "StgSharp Framework Dialogue";
            this.Add(this.tittle);
        }

        public void ShowError(string message)
        {
            this.Remove(this.tittle);
            this.ColorScheme = error;
            this.Text =
                 """
                 
                         ██╗           ███████╗██████╗ ██████╗  ██████╗ ██████╗ 
                 ██╗    ██╔╝           ██╔════╝██╔══██╗██╔══██╗██╔═══██╗██╔══██╗
                 ╚═╝    ██║            █████╗  ██████╔╝██████╔╝██║   ██║██████╔╝
                 ██╗    ██║            ██╔══╝  ██╔══██╗██╔══██╗██║   ██║██╔══██╗
                 ╚═╝    ╚██╗           ███████╗██║  ██║██║  ██║╚██████╔╝██║  ██║
                         ╚═╝           ╚══════╝╚═╝  ╚═╝╚═╝  ╚═╝ ╚═════╝ ╚═╝  ╚═╝
                 
                 """ + "\n"+
                message;

            Button button = new Button()
            {
                Text = "Click to quit."
            };
            button.Accept += (sender,e) => {
                Application.RequestStop();
            };

            this.Add(button);
            this.SetNeedsDisplay();
        }

    }
}
