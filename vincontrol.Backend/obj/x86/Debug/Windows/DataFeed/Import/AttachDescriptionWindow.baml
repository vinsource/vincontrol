   M S B A M L   `   `   `  ����  vincontrol.BackendH    Bvincontrol.Backend.Windows.DataFeed.Import.AttachDescriptionWindow   5   	   U QWPFToolkit, Version=3.5.40128.1, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   X TXceed.Wpf.Toolkit, Version=1.9.0.0, Culture=neutral, PublicKeyToken=3e4669d2f30244f44xctk)http://schemas.xceed.com/wpf/xaml/toolkit  6@     � Title$  Attach Import Description��5   	   $	��400��6+   $	��500��68    � WindowStartupLocation$ CenterScreen=�6E   .��5      � .+�5   
   �� .+�5      ��  �� Orientation$ 
Horizontal=�6   .+�5      �� $	��100��6   $��Status��6(   �� 5	      $	��200��6    �� SelectedValuePath$	 Key��6J    �� DisplayMemberPath$ Value��6c   ��6'   �� *
StatusList+ �� SelectedValue 6}   �� *CurrentStatus+ �� ValidatesOnDataErrors .  �� NotifyOnValidationError . 6�   5
      �� 5      $ 
Horizontal=�6   $��0 10 0 0q�62   .+�5      �� $	��100��6   $��Content��6(   �� 5      $	��300��6   $	��183��6?    �� AcceptsReturn . "	 �� VerticalScrollBarVisibility$
	 Auto=�6a   
 �� TextWrapping$

 Wrap=�6�   ��6&   �� *	Content+6�   5      �� 5      $ 
Horizontal=�6   $��0 10 0 0q�62   .+�5      �� $��Attach File��6   ��60   �� *AttachFileCommand+6Y   �� 5      $��	45 0 25 0q�6   $	��100��6-   {�.  ��6K   �� *
FileName+6g   �� 5      $��Download File��6   ��62   �� *DownloadFileCommand+6]   5      �� 5      $ 
Horizontal=�6   $��0 10 0 0q�62   $��Center=�6D   .+�5      �� $
��Save��6   $	��100��6L   ��6(   �� *SaveFileCommand+6Z   �� 5      $��Save and send email��6   $	��140��6g   ��67   �� *SaveFileAndSendEmailCommand+6g   5      5      %  Xceed.Wpf.Toolkit.BusyIndicator  5   
   $��Stretch=�6   $��Stretch=�6:   $��6,0,6,66q�6X      IsBusy 6j   �� *IsBusy+6�   5      5      