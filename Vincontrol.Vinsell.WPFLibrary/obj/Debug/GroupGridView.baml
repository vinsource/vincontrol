   M S B A M L   `   `   `  ����!  Vincontrol.Vinsell.WPFLibrary1    +Vincontrol.Vinsell.WPFLibrary.GroupGridView   5      \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35U QWPFToolkit, Version=3.5.40128.1, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      :custom-http://schemas.microsoft.com/wpf/2008/toolkit  5        � 	Resources  5	         GroupHeaderStyle �� 
TargetType �� Property �� Value �� 
TargetType 9�Name 9� 
IsExpanded �� Orientation �� Path��%�  &	        �� 5
   
   " ��6)   .�5      ��  � �� 5      �� 5      " ��6&   .R�5      9� $	 exp��6#    . 
�������
���   ���5      �� 5   "   $ 
Horizontal=�6-   .+�5   &   �� ��60   �� *Name+�� 5   &   $
��Bold"�6P   $��8,0,4,0q�6b   ��60   �� $ 	ItemCount�6b   �� 5   &   $��Vehicles��60   5   #   .��5      �� 5      5      5      5      5      .��5      �� .+�5    
   �� $ 
Horizontal=�6   .+�5!      �� -   5        	 ��Name$	 btnUpcoming��5!   =   $��Upcoming Auctions��6   $	��150��61   
�������6�   �� 5"      -   5        $	 btnPast��5"   :   $��Past Auctions��6   $	��150��6.   
�������6v   5#      � 5$   
   $	��500��6   .+�5%   
   �� .��5&   
   � .+�5'      )  #Microsoft.Windows.Controls.DataGrid  -   5        
  Name$
 dgData��5'         CanUserAddRows .     AutoGenerateColumns .     CanUserDeleteRows .  ��5(   4   ��  �� 
GroupStyle 5)      (  "System.Windows.Controls.GroupStyle  5*         ContainerStyle# [  6!      Panel 5+      �� 5,      .R�5-   "   A  ;Microsoft.Windows.Controls.Primitives.DataGridRowsPresenter  5.      50         Columns 52      3  -Microsoft.Windows.Controls.DataGridTextColumn  53      /  )Microsoft.Windows.Controls.DataGridColumn   Header$
 Year��60   4  .Microsoft.Windows.Controls.DataGridBoundColumn   Binding 6>   �� *Year+  54      $
 Make��60    6>   �� *Make+  55      $ Model��60    6?   �� *Model+  56      $
 Trim��60    6>   �� *Trim+  57      $	 VIN��60    6=   �� *Vin+  58      $ Seller��60    6@   �� *Seller+  59      $ Mileage��60    6A   �� *	Mileage+  5:      $	 MMR��60    6=   �� *Mmr+  5;      $
 Lane��60    6>   �� *Lane+  5<      $	 Run��60    6=   �� *Run+  5=      $ 	Condition��60    6C   �� *Cr+5?      5@      5A      5B      5C      5D      