   M S B A M L   `   `   `  ���� \  XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35^ ZWindowsFormsIntegration, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089 
  btn_UI  �� 
TargetType ��Name �� CornerRadius � Cursor �� ContentStringFormat �� ContentStringFormat J� SnapsToDevicePixels �� Triggers P� Property	 P� Value
 �� 
TargetName �� Property �� Value  ControlBorderBrush �� Color  FocusBorderBrush  contentCenterStyle �� 
TargetType  SimpleProgressBar �Name �� Angle  TextControlBorderBrush �� EndPoint �� 
StartPoint � Color � Offset-    'System.Windows.Input.KeyboardNavigation  
WhiteColor � 	Resources  Key g� Duration �� 
TargetName �� TargetProperty	 W� To g� 	BeginTime O� KeyTime O� Value R� KeyTime  System.Windows.Visibility   Visible R� Value  
FocusBrush  J� Opacity 	 MouseOverHighlightBrush! J� IsHitTestVisible" ��Name 
 OutsideFontColor# N� ExitActions$ ��Name  
FocusedOff% N� EnterActions  	FocusedOn& ]� 
Conditions' �� Property( �� Value  HoverOff  HoverOn  
DisabledOn  DisabledForegroundBrush&    System.Windows.Controls.DataGrid=   7System.Windows.Controls.Primitives.DataGridColumnHeader) � ColumnDefinitions* F�Name+ �� Stretch, J� RenderTransformOrigin)   #System.Windows.Controls.DataGridRow- �� Triggers*   $System.Windows.Controls.DataGridCell. �� TextAlignment  Btn_InfoTemplate  Btn_DeleteTemplate  Btn_CancelTemplate  Btn_SelectTemplate  Btn_ItemTemplate�� F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation      :x,http://schemas.microsoft.com/winfx/2006/xaml      %�  &	        &	     &	 (    &	 ;    &	 �    7  7  &	 �    '�� �    7  7  7  7  7  7  '  8    '      '      '  b    &	     &	 V    &	 :    &	     &	     �� "  ��.R��� $ 
Btn_Border��#��z��$ 0��$
 Hand��._��� #��z��#��z��# z #��z��#��z����. # z #��z�� P�  �@J�IsMouseOver	 . . ��� $
 
Btn_Border�� � �� �#334d5cP�  � {�	 .  . ��� $
 
Btn_Border�� � �� �#888888�� $ #999C9F���� $ #EBCECE���� " ��.���  � ��$ Center=���  � ��$ Center=���  �@��TextAlignment$ Center=���  � ��$ 20(���  � ��$ 5,0,0,0q��� " �.���  � ��8   ��  �@�Maximum$ 1Q���  �@�Value$ 0Q���  � ��$ 10����  � �� .  ��  � ��8  ��  �@J�SnapsToDevicePixels . ��  � �� �� "  �.R�� $
 Root��.+��� $ 
PART_Track��#��z��# z �� $ PART_Indicator��$
��Left=�#��z��# z  P�  �@�Orientation$	 Vertical=�. ��� $

 Root�� �@�LayoutTransform �� $	 -90Q��� $ 0.5,1&�$ 0.5,0&�.��� $ #CACACA��� $ #CACACA��$ 1Q��� " ��.���  �@  TabNavigation$
 None=���  �@�FocusVisualStyle Z� ��  �@J�	AllowDrop . ��  � �� �� # �  ��  � ��$ Stretch=���  � ��$ Stretch=���  � ��$ Trebuchet MS)���  � ��$ 12(���  � ��$ 4q���  � ��$ 1q���  � ��8   ��  � �� �� "  �� ���� HoverOn .��W� $ 00:00:00.1000000D�$
 Over��$ Opacity�$ 1X�W� $ 00:00:00.1000000D�$ Over_Border��$ Opacity�$ 1X��� HoverOff .��W� $ 00:00:00.4000000D�$
 Over��$ Opacity�$ 0X�W� $ 00:00:00.4000000D�$ Over_Border��$ Opacity�$ 0X��� 	FocusedOn .��U� $ 00:00:00X�$ FocusVisualElement��$ (UIElement.Opacity)�.\��� $ 00:00:00.1000000��$ 1Q��� 
FocusedOff .��U� $ 00:00:00X�$ FocusVisualElement��$ (UIElement.Opacity)�.\��� $ 00:00:00.3000000��$ 0Q��� 
DisabledOn .��T� $ 00:00:00X�$ DisabledVisualElement��$ (UIElement.Visibility)�..�i� $ 00:00:00.1000000��# Z .R�� .+��� $ 
BorderBase��#��z��#��z��#��z��$
 2.75���� $
 Over��#���  #��z��$
 2.75��$  0Q��� $ Over_Border��$��-1q�#��� 	 #��z��$
 3.75��! .  $  0Q��� $" PART_ContentHost��$��0q�#��� 
 �� $ DisabledVisualElement�����	#A5FFFFFF���	#59C0C0C0#��z��$
 2.75��! .  $y�	Collapsed=��� $ ReadOnlyVisualElement�����	#66FFFFFF$
 2.75��$y�	Collapsed=��� $ FocusVisualElement�����	#FFB1703C#��z��$
 2.75��! .  $  0Q� P�  �@J�	IsFocused	 . # �� $ $ FocusedOff_BeginStoryboard��8�� % �� $$ FocusedOn_BeginStoryboard��8�� ]� & �� ' �@J�IsMouseOver( . �� ' �@J�	IsFocused( .  # �� $$ HoverOff_BeginStoryboard��8�� % �� 8�� P�  � {�	 .  % �� $ $ DisabledOn_BeginStoryboard��8�� . ��� # �   � ���� "  .���  �@ CanUserResizeColumns .  ��  � �� �#FFFFFF��  � �� �#ddd��   �@ HorizontalGridLinesBrush �� $
 #ddd����  �@ VerticalGridLinesBrush �� $
 #ddd���� "  .���  �@J�SnapsToDevicePixels . ��  � �� �#000��  � ��$ 16(���  �@�Cursor$
 Hand����  � �� �� "   .R��� $ BackgroundBorder��$��0,0,0,2q����#ddd$
��Auto��._�� ) �� $��* �.+��� $��10q�$��Center=�$
��Left=�F� $* 	SortArrow��$y�	Collapsed=�"����        �@B     � � @B 	$
+ Fill=�$��2��$��8��$��6��
�������$��0,0,50,0q�$��Center=�$	, 1,1&��� $��1�����#ddd$��Right=�$��1����  � ��$ 25���� "  .���  � �� �#333333��  � ��$ 13(�- P�  �@ AlternationIndex$	 0��. ���  � �� �#f9f9f9P�  �@ AlternationIndex$	 1��. ���  � �� �#FFFFFFP�  �@J�IsMouseOver	 . . ���  � �� �#F9F9F9P�  �@ 
IsSelected	 . . ���  � �� �#f5f5f5�� "  .���  � �� �� "   .R��� $. Center=�$��14(�$��Center=�$
��Left=�$��10,10q��� - P�  �@ 
IsSelected	 . . ���  � ��
 �   ��� "  ��.R��� $ 
Btn_Border�����#52baeb$ 2��._��� $��Center=����#FFFFFF$	��8,4q�$��12(�#��z��$��Center=�$
 Hand�� P�  �@J�IsMouseOver	 . . ��� $
 
Btn_Border�� � �� �#2f96b4P�  � {�	 .  . ��� $
 
Btn_Border�� � �� �#888888��  �@�Cursor$
 None���� "  ��.R��� $ 
Btn_Border�����#d15b47$ 2��._��� $��Center=����#FFFFFF$	��8,4q�$��12(�#��z��$��Center=�$
 Hand�� P�  �@J�IsMouseOver	 . . ��� $
 
Btn_Border�� � �� �#bd362f�� "  ��.R��� $ 
Btn_Border�����#888888$ 2��._��� $��Center=����#FFFFFF$	��8,4q�$��12(�#��z��$��Center=�$
 Hand�� P�  �@J�IsMouseOver	 . . ��� $
 
Btn_Border�� � �� �#d15b47�� "  ��.R��� $ 
Btn_Border�����#006dcc���#0062b7$��1q�$ 3��._��� $��Center=����#FFFFFF$	��8,4q�$��12(�#��z��$��Center=�$
 Hand�� P�  �@J�IsMouseOver	 . . ��� $
 
Btn_Border�� � �� �#0044cc�� "  ��.R��� $ 
Btn_Border�����#108ed8���#108ed8._��� $��Center=����#FFFFFF$	��8,4q�$��14(�#��z��$��Center=�$
 Hand�� P�  �@J�IsMouseOver	 . . ��� $
 
Btn_Border�� � �� �#0c79b1