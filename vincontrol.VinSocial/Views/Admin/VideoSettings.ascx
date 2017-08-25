<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<dynamic>" %>
<div class="admin_videosettings_holder admin_holder">
    <div class="admin_video_left">
        <div class="av_title">
            Audio Settings</div>
        <div class="av_audio_settings">
            <div class="av_voice_select_holder">
                <div class="av_voice_select">
                    <select>
                        <option>Select Voice</option>
                    </select>
                </div>
                <div class="av_preview">
                    <img src="../../Content/Images/admin/preview.png" />
                </div>
            </div>
            <div class="av_des_holder">
                <textarea></textarea>
            </div>
        </div>
        <div class="av_title">
            Video Topics</div>
        <div class="av_video_topics">
            <div class="av_video_items">
                <label>
                    Dealership Description
                    <input type="checkbox" />
                </label>
            </div>
            <div class="av_video_items">
                <label>
                    Packages
                    <input type="checkbox" />
                </label>
            </div>
            <div class="av_video_items">
                <label>
                    Standard Options
                    <input type="checkbox" />
                </label>
            </div>
            <div class="av_video_items">
                <label>
                    Additional Options
                    <input type="checkbox" />
                </label>
            </div>
            <div class="av_video_items">
                <label>
                    Carfax
                    <input type="checkbox" />
                </label>
            </div>
            <div class="av_video_items">
                <label>
                    Leasing Information
                    <input type="checkbox" />
                </label>
            </div>
        </div>
    </div>
    <div class="admin_video_right">
        <div class="av_right_intro">
            <div class="av_title">
                Intro/outro Images</div>
            <div class="av_right_upload_holder">
                <div class="av_right_upload_text">
                    Upload an image for the start and end of your video so that your dealership is represented!</div>
                <div class="av_right_upload_icons">
                    <div class="av_right_upload_icon">
                        <img src="../../Content/Images/admin/download.png" />
                    </div>
                      <div class="av_right_upload_icon">
                        <img src="../../Content/Images/admin/download.png" />
                    </div>
                </div>
            </div>
        </div>
        <div class="av_right_video">
        <div class="av_title">
                Video Example</div>
        <iframe width="589" height="370" src="http://www.youtube.com/embed/I8lK44IU_3A?autoplay=1">
        </iframe>
        </div>
    </div>
</div>
