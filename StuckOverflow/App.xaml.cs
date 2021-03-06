using PRBD_Framework;
using prbd_1920_xyy.Properties;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace prbd_1920_xyy {
    public enum AppMessages {
        MSG_DISPLAY_QUESTION,
        MSG_DISPLAY_BYTAG,
        MSG_DELETE_VIEUW,
        MSG_ADD_COMMENT,
        MSG_EDIT_COMMENT,
        MSG_EDIT_ANSWER,
        MSG_EDIT_QUESTION,
        MSG_REFRESH_QUESTION
    }

    public partial class App : ApplicationBase {
        public static Model Model { get; private set; } = new Model();

        public static readonly string IMAGE_PATH =
            Path.GetFullPath(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "/../../images");
        public static User CurrentUser { get; set; }

        public static void CancelChanges() {
            Model.Dispose(); // Retire de la mémoire le modèle actuel
            Model = new Model(); // Recréation d'un nouveau modèle à partir de la DB
        }
        public App() {
            Thread.CurrentThread.CurrentUICulture = new CultureInfo(Settings.Default.Culture);
            ColdStart();
        }

        private void ColdStart() {
            Model.SeedData();
        }
    }
}
