   M S B A M L   `   `   `  ����p  lSystem.Windows.Controls.Input.Toolkit, Version=3.5.40128.1, Culture=neutral, PublicKeyToken=31bf3856ad364e35pTclr-namespace:System.Windows.Controls;assembly=System.Windows.Controls.Input.ToolkitSystem.Windows.Controls  5   	   M Ivincontrol.Backend, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullE'clr-namespace:vincontrol.Backend.Helpervincontrol.Backend.Helper 5   @   C   =vincontrol.Backend.Windows.DataFeed.ViewDealerOfProfileWindow   5   	   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35U QWPFToolkit, Version=3.5.40128.1, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   cmyTclr-namespace:System.Windows.Controls;assembly=System.Windows.Controls.Input.Toolkit     5   	   X TXceed.Wpf.Toolkit, Version=1.9.0.0, Culture=neutral, PublicKeyToken=3e4669d2f30244f44xctk)http://schemas.xceed.com/wpf/xaml/toolkit  5   	   4helper'clr-namespace:vincontrol.Backend.Helper  6@     � WindowStartupLocation$  CenterScreen=�5   	    � Title$- 'Dealer Management for Importing Profile��5   	   $	��700��69   $
��1000��6F   .��5      � $	��711��6   $��Stretch=�6   .+�5	   
   �� $��0,0,0,54q�6   .+�5
      &   System.Windows.Controls.DataGrid     CanUserAddRows .     AutoGenerateColumns .  $	��315��6�   $��Stretch=�6�   $��12(�6�   2  ,vincontrol.Backend.Helper.DataGridExtensions   DataContextForColumns 6   ��  �� Mode$ OneWay=���6�   �� *SelectedDealers+   Columns 5      4  .System.Windows.Controls.DataGridTemplateColumn  5         CellTemplate 5      �� 5      .R�5   "   �� $��
linkDelete��6  $��40��6)   $��20��64   $��Right=�6�   $	��Top=�6  ��.  ��6@   ��  �� RelativeSource �� *FindAncestor+	 �� AncestorType"	  
 �� Path$+
 %DataContext.DeleteImportDealerCommand���6�   �� ��5   &   �� 5   *   $��5,0,5,0q�60   $��Center=�6A   $��Uniform=�6\    � Cursor$
 Hand��6o   $ ��../../../Images/delete.png��6}   5   #   5      5        5      ,  &System.Windows.Controls.DataGridColumn   Width5  /System.Windows.Controls.DataGridLengthConverter$ 75 6.      Header$ Discontinue��6:    5      �� 5      .R�5   "   ��  d� 	IsChecked 6+   �� *Discontinued+$ TwoWay=� �� UpdateSourceTrigger$ PropertyChanged=�6�   5      5      0  *System.Windows.Controls.DataGridTextColumn  5      $ Dealer Name��6)   $	 120 6>      FontSize$ 13Q�6o   1  +System.Windows.Controls.DataGridBoundColumn   Binding 6K   �� $
 
DealerName�6o     5      $	 270 6.   $ Feed Url��6;    5       �� 5!      .R�5"   "   �� ��6*   �� *	FeedUrl+$ TwoWay=�$ PropertyChanged=�6w   5#      5%        5&      $ Last Deposited Date��6)   $	 200 6F      
Visibility 6R   �� *DataContext.IsBundle+ �� *Self+   HiddenConverter �� 	Converter# [   6�   �� $
 LastDepositedDate�  5(      $	 150 6-   $ File URL��69    6K   �� *DataContext.IsBundle+ �� *Self+# [   5)      �� 5*      .R�5+   "   �� $��23��6)   $	��150��67   ��6C   ��  �� *FindAncestor+"	  $%
 DataContext.DownloadFileCommand���6�   �� *	FileUrl+��6�   �� *	FileUrl+6  5,      5.      5H      � 5I      -   5        $��
wrapPanel1��5I   #   $��8��6   $	��844��65   6C   � 5J      -   5        $��
wrapPanel2��5J   $   $��26��6   $	��845��66   .+�5L      �� $��Import Manually��6   $	��134��63   $��16(�6i   ��6?   �� *ImportManuallyCommand+6y   �� 5M      $��ShowHistory��6   $	��134��6/   $��16(�6b   ��6;   �� *ShowHistoryCommand+6r   �� 5O      $��Apply��6   $	��134��6)   $��16(�65   ��6C   �� *ApplyCommand+y�6d   �� *TaskExisted+# [  6�   �� 5P      $��Save Changes��6   $	��134��60   $��16(�6<   ��6J   �� * SaveDealerImportProfileCommand+y�6}   �� *TaskExisted+  VisibilityConverter# [ 6�   5Q      �� 5R      $��	0 20 0 0 q�6   
�������$	��292��6:   ._�5S      �� .+�5T      �� $��Search Dealer��6    $
��Bold"�65   $��15(�6G   6W   �� 5U       �� Orientation$ 
Horizontal=�6!   .+�5V      �� -   5        $��	txtSearch��5V   �   $	��200��6f   $��5q�6r   $��14(�6}   ��6"   �� *SearchContent+$ PropertyChanged=�.��6�    J� InputBindings 5W      �� 5X   "   
 �� Key$ Enter��6-    �� Command 69   �� *SearchDealerCommand+6d   .��5Y   5   5Z      �� 5[      -   5        $��	btnSearch��5[   �   $��Search��6!   $	��100��6[   $��5q�6g   $��14(�6r   ��62   �� *SearchDealerCommand+6�   5\        5^       .   .  $	��224��6R   $	��836��6`   ��6l   �� *SearchResultDealers+ 5_        5`      $ Dealer Name��61   $	 300 6F   $ 14Q�6R    6b   �� $

 Name�  5a      $	 150 65    5b   "   �� 5c   &   .R�5d   *   �� $��
Add Dealer��61   $	��100��6F   $��20��6R   $��Right=�6  $	��Top=�6(  ��.  ��6^   ��  �� *FindAncestor+"	  $.
 (DataContext.AssignDealerToProfileCommand���6�   �� 5e   +   5f   '   5h      5j      5o      5p      5s      %  Xceed.Wpf.Toolkit.BusyIndicator  5t   
   $��Stretch=�6   $��Stretch=�6:   $��6,0,6,66q�6X      IsBusy 6j   �� *IsBusy+6�   5u      5x      