   M S B A M L   `   `   `  ����  vincontrol.BackendK    Evincontrol.Backend.Windows.DataFeed.Export.ExportFilenameFormatWindow   5   	   U QWPFToolkit, Version=3.5.40128.1, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	     � WindowStartupLocation$  CenterScreen=�6@    � Title$ 	File Name��5   	   $	��300��6   $	��600��6(   .��5      �� .+�5   
   ��  �� Orientation$ 
Horizontal=�6   60   �� 5   
    � DataContext 6   �� *OtherFileNameFormat+.+�5      	�  	� 	GroupName$ FileNameGroup��6   $��14(�6�    d� 	IsChecked 65   ��  �� Path$ 
IsSelected� �� UpdateSourceTrigger$ PropertyChanged=� �� Mode$ TwoWay=���5	      �� 5
      .+�5      �� $��Other��6$   63   �� 5      $	��200��6"   ��6/   �� *Value+6H   5      5      5      �� 5   
   -   5        $��listBoxZone��5      $	��184��6P   $��14(�6]   ��6%   �� *FileNameFormatList+��5      �� 5      .R�5      �� $ 
Horizontal=�6!   .+�5      � $	��200��6   .+�5      	� $ FileNameGroup��6*    6E   �� $ 
IsSelected�$ PropertyChanged=�$ TwoWay=���6�   �� *Text+5      �� 5      $	��300��6$   ��60   �� *	Example+6K   5      5      5      �� 5   
   $ 
Horizontal=�6   .+�5      �� -   5        $��btnOk��5   L   $
��Save��6   $��18(�6Y   ��6$   �� *SaveFileNameCommand+6i   �� 5       -   5        $��	btnCancel��5    &   $��Cancel��6   $��18(�6O   6_   5!      5$      5<      