   M S B A M L   `   `   `  ����|  xSystem.Windows.Controls.DataVisualization.Toolkit, Version=3.5.40128.1, Culture=neutral, PublicKeyToken=31bf3856ad364e35�{clr-namespace:System.Windows.Controls.DataVisualization.Charting;assembly=System.Windows.Controls.DataVisualization.Toolkit2System.Windows.Controls.DataVisualization.Charting  5      O Kmscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e0891&clr-namespace:System;assembly=mscorlibSystem 5      X TVincontrol.Vinsell.WPFLibrary, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullc6clr-namespace:Vincontrol.Vinsell.WPFLibrary.Converters(Vincontrol.Vinsell.WPFLibrary.Converters 5      �rclr-namespace:System.Windows.Controls.DataVisualization;assembly=System.Windows.Controls.DataVisualization.Toolkit)System.Windows.Controls.DataVisualization  5	      0   *Vincontrol.Vinsell.WPFLibrary.ChartControl   5      \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35U QWPFToolkit, Version=3.5.40128.1, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      �chartingToolkit{clr-namespace:System.Windows.Controls.DataVisualization.Charting;assembly=System.Windows.Controls.DataVisualization.Toolkit     5      9system&clr-namespace:System;assembly=mscorlib     5      Hlocal6clr-namespace:Vincontrol.Vinsell.WPFLibrary.Converters     5      �visualizationToolkitrclr-namespace:System.Windows.Controls.DataVisualization;assembly=System.Windows.Controls.DataVisualization.Toolkit     5	        � 	Resources  5         	PlotStyle �� 
TargetType>   8System.Windows.Controls.DataVisualization.Charting.Chart �� Property �� Value �� Color �� Opacity  OverYellowBrush  OverGreenBrush  UnderYellowBrush  UnderRed  ScatterPositiveI   CSystem.Windows.Controls.DataVisualization.Charting.ScatterDataPoint �� 
TargetType  ComparedPointE  ?Vincontrol.Vinsell.WPFLibrary.Converters.FormatMileageConverter  FormatMileageC  =Vincontrol.Vinsell.WPFLibrary.Converters.FormatPriceConverter  FormatPrice��%  &	        &	 �     &	     &	 <    &	 \    &	 y    &	 "    &	 �    &	 �    �� 5   
   "  6"   .�5      ��  �@ PlotAreaStyle 5      �� 5      " �6   .�5      ��  � �� 5      �� 5   "   $	 Red��62   $	 0.1Q�6>   6N   5      5      5      5      �� 5   
   $ Yellow��62   $	 0.3Q�6A   �� 5   
   $ Green��61   $	 0.7Q�6?   �� 5   
   $ Yellow��63   �� 5   
   $	 Red��6+   �� 5    
   "  5!   	   .�5#      ��  � �� 5%      �� 5'      "  6&   .R�5(      A� 
���   �
�������$��1��6=   5)      5-      5/      �� 50   
   "  51   	   .�52      ��  � ��$ 20��6&   �� 53       � ��$ 20��6'   55        56   
   6'     57   
   6%   5:      .��5<      � $	��921��6   
������� � ColumnDefinitions 5=   
   �� 5>      6!   �� 5?      $	��220 �6   6-   .+�5A   
     -   5        $��	lineChart��5A       $	��Top=�62   $��1,2,0,0q�6J   $	��440��6[      LegendStyle 5B      �� 5C      " ��6   .�5D      ��  � ��$ 0��6/   �� 5E       � ��$ 0��6.   5F      	   Axes	 5I      C   =System.Windows.Controls.DataVisualization.Charting.LinearAxis  =   7System.Windows.Controls.DataVisualization.Charting.Axis
   Orientation$
 Y=�6-   D   >System.Windows.Controls.DataVisualization.Charting.NumericAxis   Minimum�   �System.Windows.Controls.DataVisualization.Charting.NullableConverter`1[[System.Double, mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089]]$ 0 6=   D	   >System.Windows.Controls.DataVisualization.Charting.DisplayAxis 	  Title$ Price($)��6I   $��16(�6Z    	  AxisLabelStyle 5J      �� 5K      B
   <System.Windows.Controls.DataVisualization.Charting.AxisLabel" 
 6    .�5L      ��  � �� 5M   "   �� 5N   &   " 
 66   .R�5O   *   �� ��65   �� *FormattedContent+ �� 	Converter# [ 5P   '   5R      5S      5U        5V      $
 X=�6-   $ 0 6=   $ Odometer(Miles)��6I   $��16(�6a    5W      �� 5X      " 
 6    .�5Y      ��  � �� 5Z   "   �� 5[   &   " 
 66   .R�5\   *   �� ��65   �� *FormattedContent+# [ 5]   '   5_      5`      5b         Series. 5e      C   =System.Windows.Controls.DataVisualization.Charting.LineSeries  H   BSystem.Windows.Controls.DataVisualization.Charting.DataPointSeries   DependentValuePath$ Price��6+   
���  ��   IndependentValuePath$ Mileage��5f         IsSelectionEnabled .    ItemsSource 64   �� *[0]+5g        5h      $ Price��6+   
��� ���$ Mileage��5i       .  64   �� *[1]+5j        5k      
��� � �$ Price��6>   $ Mileage��5l       .  64   �� *[2]+5m        5n      $ Price��6+   
��� ���$ Mileage��5o       .  64   �� *[3]+5q        5r      $ Price��6,   
���  ��$ Mileage��5s       .  64   �� *[4]+5u      F   @System.Windows.Controls.DataVisualization.Charting.ScatterSeries  5v      -   5        
�������$ Price��5v   ?   $ Mileage��5w       .    DataPointStyle# [ 5x   0    5w   4   �� *[5]+5z        5{      # [ 6H   $ Price��6y   $ Mileage��5|       5{   ,   �� *[6]+5~         Palette 5�      L   FSystem.Windows.Controls.DataVisualization.ResourceDictionaryCollection   	 DataPointStyle�� 5�      %T   &		       �� 5�      " ��67   .�5�      ��  � ��
 �  ��6;   5�      5�      �� 5�      %T   &		       �� 5�      " ��67   .�5�      ��  � ��
 � ���6;   5�      5�      �� 5�      %T   &		       �� 5�      " ��67   .�5�      ��  � ��
 � � �6;   5�      5�      �� 5�      %T   &		       �� 5�      " ��67   .�5�      ��  � ��
 � ���6<   5�      5�      �� 5�      %T   &		       �� 5�      " ��67   .�5�      ��  � ��
 �  ��6;   5�      5�      �� 5�      %T   &		       �� 5�      " ��67   .�5�      ��  � ��
 �   �6;   5�      5�      5�      5�      �� 5�   
   
�������$��1��6-   .+�5�      �� -   5         ��Name$ VehicleDetail��5�   A   $��5q�6   
����ح�._�5�      �  � Orientation$ Vertical=�6   .+�5�      �� 
������� �� Orientation$ 
Horizontal=�65   .+�5�      � $��5q�6%   $ 
Horizontal=�60   $	��199��6I   .+�5�      �� $��15(�6(   $
��Wrap=�6M   ��66   �� *Title+6d   5�      5�      �� 5�      
�������$ 
Horizontal=�65   .+�5�      �� $��5q�6!   $	��150��6L   $	��100��6X   ��6,   �� *ThumbnailURL+6g   5�      �� 5�      
�������.+�5�      �� $��5q�6%   $
��Wrap=�60   �� 5�      ��6    �� *AutoTraderListingURL+.�6N   AutoTrader Listing6b   5�      5�      �� 5�      
�������$ 
Horizontal=�65   $��Center=�6N   .+�5�      �� $��Price: ��6$   $��61��63   $��5q�6>   $��15(�6I   6Y   �� 5�      $
��Wrap=�6$   $	��138��68   ��6D   �� *Price+6^   5�      �� 5�      
�������$ 
Horizontal=�64   $��Center=�6M   .+�5�      �� $��5q�6%   $
��Wrap=�60   
���  ��$��Miles: ��6U   $��61��6d   $��15(�6o   6   �� 5�      $	��138��6(   $
��Wrap=�64   
���  ����6Y   �� *	Mileage+6t   5�      �� 5�      
�������$ 
Horizontal=�65   $��Center=�6N   .+�5�      �� $��5q�6%   $
��Wrap=�60   $��Seller: ��6D   $��61��6T   $��15(�6_   6o   �� 5�      $
��Wrap=�6$   $	��138��68   ��6D   �� *Seller+6^   5�      �� 5�      
�������$ 
Horizontal=�65   $��Center=�6N   .+�5�      �� $��5q�6%   $
��Wrap=�60   $��	Address: ��6D   $��61��6U   $��15(�6`   6p   �� 5�      $
��Wrap=�6$   $	��138��68   ��6D   �� *SellerAddress+6f   5�      �� 5�      
�������$ 
Horizontal=�65   $��Center=�6N   .+�5�      �� $��5q�6%   $��
Distance: ��60   $��61��6C   $��15(�6N   6^   �� 5�      $
��Wrap=�6$   $	��138��68   ��6D   �� *
Distance+6a   5�      5�      5�      �� 5�      -   5        $ CurrentVehicle��5�   A   $��5q�6   
����ح�._�5�      � 
�������$ 
Horizontal=�6/   .+�5�      �� 
�������$ Vertical=�65   .+�5�      � $��5q�6&   $ 
Horizontal=�61   $	��199��6J   .+�5�      �� $��15(�6(   $
��Wrap=�67   ��6L   �� *Title+6e   5�      �� 5�      
�������$ 
Horizontal=�69   $��Center=�6R   .+�5�      �� $��15(�6(   $��5q�67   $��Price: ��6B   $��61��6Q   6^   �� 5�      $
��Wrap=�6(   $	��138��6<   ��6H   �� *Price+6b   5�      �� 5�      
�������$ 
Horizontal=�68   $��Center=�6Q   .+�5�      �� $��15(�6(   $��5q�66   $
��Wrap=�6A   $��Miles: ��6V   $��61��6e   6r   �� 5�      $	��138��6(   $
��Wrap=�64   ��6H   �� *	Mileage+6c   5�      5�      5�      5�      5�      5�      5�      