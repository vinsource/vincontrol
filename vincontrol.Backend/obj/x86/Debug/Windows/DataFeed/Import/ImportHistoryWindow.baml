   M S B A M L   `   `   `  ����  vincontrol.BackendD    >vincontrol.Backend.Windows.DataFeed.Import.ImportHistoryWindow   5   	   U QWPFToolkit, Version=3.5.40128.1, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   X TXceed.Wpf.Toolkit, Version=1.9.0.0, Culture=neutral, PublicKeyToken=3e4669d2f30244f44xctk)http://schemas.xceed.com/wpf/xaml/toolkit  6@     � Title$  ImportHistoryWindow��5   	   $
��1000��6%   $
��1000��63   .��5      �� .��5   
   � .+�5      �� .+�5      �� $��	Inventory��6    �� Padding$ 10 20q�6-   6?   &   System.Windows.Controls.DataGrid  5	         CanUserAddRows .     AutoGenerateColumns .  $��Stretch=�6N   $��Stretch=�6j   $��14(�6�   ��6�   �� *Inventories+   Columns 5
      0  *System.Windows.Controls.DataGridTextColumn  5      ,  &System.Windows.Controls.DataGridColumn   Header$	 ACV��6-   1  +System.Windows.Controls.DataGridBoundColumn   Binding 6;   ��  �� Path$	 ACV�  5      $ Car Image Url��6-    6E   �� $ CarImageUrl�  5      $ Cars Options��6-    6D   �� $ CarsOptions�  5      $ Cars Packages��6-    6E   �� $ CarsPackages�  5      $ Date In Stock��6-    6E   �� $ DateInStock�  5      $ Dealer MSRP��6-    6C   �� $ 
DealerMSRP�  5      $ Default Image Url��6-    6I   �� $ DefaultImageUrl�  5      $
 Make��6-    6<   �� $
 Make�  5      $
 Year��6-    6<   �� $ 	ModelYear�  5      $
 MSRP��6-    6<   �� $
 MSRP�  5      $ Model��6-    6=   �� $ Model�  5      $ 
Sale Price��6-    6B   �� $ 	SalePrice�  5      $ Standard Options��6-    6H   �� $ StandardOptions�  5      $
 Trim��6-    6<   �� $
 Trim�  5      $	 VIN��6-    6;   �� $ 	VINNumber�5#      �� 5$      $��Soldout Inventory��6   $ 10 20q�65   6G     5&       .   .  $��Stretch=�6N   $��Stretch=�6j   $��14(�6�   ��6�   �� *SoldoutInventories+ 5'        5(      $	 ACV��6-    6;   �� $	 ACV�  5)      $ Car Image Url��6-    6E   �� $ CarImageUrl�  5*      $ Cars Options��6-    6D   �� $ CarsOptions�  5+      $ Cars Packages��6-    6E   �� $ CarsPackages�  5,      $ Date In Stock��6-    6E   �� $ DateInStock�  5-      $ Dealer MSRP��6-    6C   �� $ 
DealerMSRP�  5.      $ Default Image Url��6-    6I   �� $ DefaultImageUrl�  5/      $
 Make��6-    6<   �� $
 Make�  50      $
 Year��6-    6<   �� $ 	ModelYear�  51      $
 MSRP��6-    6<   �� $
 MSRP�  52      $ Model��6-    6=   �� $ Model�  53      $ 
Sale Price��6-    6B   �� $ 	SalePrice�  54      $ Standard Options��6-    6H   �� $ StandardOptions�  55      $
 Trim��6-    6<   �� $
 Trim�  56      $	 VIN��6-    6;   �� $ 	VINNumber�5@      5A      %  Xceed.Wpf.Toolkit.BusyIndicator  5B      $��Stretch=�6!   $��Stretch=�6>   $��6,0,6,66q�6\      IsBusy 6n   �� *IsBusy+6�   5D      5E      5F      