   M S B A M L   `   `   `  ����  vincontrol.BackendQ    Kvincontrol.Backend.Windows.DataFeed.Import.PopupChildWindow.ConditionWindow   5   	   U QWPFToolkit, Version=3.5.40128.1, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	     � WindowStartupLocation$  CenterScreen=�5   	   $	��700��5   �   $
��1000��6�    � Title 6	   ��  �� RelativeSource �� *FindAncestor+ �� AncestorType" �� �� Path$ DataContext.XMLField� � 	Resources 5         ConditionListDataTemplate �� Padding � RowDefinitions �� Orientation	 �� SelectedValuePath
 �� DisplayMemberPath �� SelectedValue �� ValidatesOnDataErrors �� NotifyOnValidationError��%:  &	        �� 5   
   .R�5      �� 
�������$ 10q�6'   $��10q�64   $��Stretch=�6@   ._�5	      �  5
      �� 5      6*   .+�5      �� $��0��6!   $ 
Horizontal=�6/   .+�5      �� $��5,5,25,5q�6#   $	��150��65   $		 Key��6j   $
 Value��6�   ��6E   �� *OperatorList+ 6�   �� *
Operator+ .  . 6�   �� 5      $��5,5,25,5q�6"   $	��150��64   ��6D   �� *ComparedValue+ .  . 6�   �� 5      $
��Bold"�6$   $��5q�66   $
��Then��6A   6O   �� 5      $��5,5,25,5q�6#   $	��150��65   $
	 Text��6�   $

 Text��6�   ��6F   ��  �� *FindAncestor+" ��$ DataContext.DBFields� 6�   �� *	DBField+ .  . 6W  �� 5      $
��Bold"�6$   $��5q�66   $��Equal��6A   6P   �� 5      $��5,5,25,5q�6"   $	��150��64   ��6D   �� *TargetValue+ .  . 6�   �� 5      $��Delete��6!   $��5q�62   ��6>   ��  �� *FindAncestor+" ��$2 ,DataContext.DeleteConditionExpressionCommand���6�   �� 6�   5      5      5      5      5      .��5      � .+�5   
   �� .��5      �� .+�5      �� $ 
Horizontal=�6   $��5q�66   .+�5       ��  �� Padding$ 5q�6    $��Data Feed Field: ��6,   $
��Bold"�6F   6Z   �� 5!      $ 5q�6    ��6,   �� *
XMLField+6H   5#      �� 5$      $��Add condition��6K   $	��100��6c   $��5q�6o   ��6   �� *AddConditionExpressionCommand+6|   �� 5%      ��6   �� *
XMLField+68   �� 5&      #��[  6   ��6T   �� *
Conditions+�� 5'      $��5q�6   $��Save Changes��6Q   $	��150��6h   $��5q�6u   ��6$   �� *SaveConditionListCommand+6�   5(      5)      5*      5+      