/*
 * UpdateCheckForm.cs
 * Copyright © 2013 kbinani
 *
 * This file is part of cadencii.
 *
 * cadencii is free software; you can redistribute it and/or
 * modify it under the terms of the GPLv3 License.
 *
 * cadencii is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.
 */
using System;

namespace cadencii.ui
{
    interface UpdateCheckForm
    {
        void showDialog(object parent);
        void setMessage(string message);
        void setDownloadUrl(string url);
        void setFont(System.Drawing.Font font);
        void setOkButtonText(string text);
        void setTitle(string title);
        void close();
        bool isAutomaticallyCheckForUpdates();
        void setAutomaticallyCheckForUpdates(bool check_always);
        void setAutomaticallyCheckForUpdatesMessage(string message);
        event EventHandler downloadLinkClicked;
        event EventHandler okButtonClicked;
    }
}
