   M S B A M L   `   `   `  ����  FrontCrypto0    *PROJET_WPF_PROMEO_210624_CLIENT.MainWindow   5   	   W SPresentationCore, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5   	   R NSystem.Xaml, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	     � Title$  PromeoCrypto ��5   	   $	��300��6   $	��335��6,   ���	#FFFF4D4D.��5      � $��5q�6    � ColumnDefinitions 5   
   �� 5      $	��10* �6   �� 5	      $	��59* �6    � RowDefinitions 5   
   �� 5      $
��Auto �6   �� 5      $
��Auto �6   �� 5      $��* �6   �� 5      $
��Auto �6   .+�5   
   �� $��Center=�6   $��5,0,5,0q�60   $��2��6A   .+�5      �� $%��Saisir la chaîne à chiffrer :��6   $��Center=�6<   $��20(�6Y   �� 5      -   5         ��Name$ Zone_Texte_Utilisateur��5      $��Stretch=�66   $��14(�6T   $��5,5,5,5q�6b   ���	#FFE9E9E96�   5      �� 5   
   $��1��6   $��Méthode de chiffrement��6    $��Center=�6@   $��5,0,5,0q�6[   $��15(�6l   $��2��6z   .��5      ��  �� Orientation$ 
Horizontal=�6   $��Center=�62   .+�5      	� -   5         	�Name$ Bouton_Caesar��5      $��César��65   $
��10,0q�6E    d� 	IsChecked$
 TrueY�6S   	� 5      -   5        $ Bouton_Playfair��5      $��Playfair��67   $
��10,0q�6J   	� 5      -   5        $ Bouton_Substitution��5      $��Substitution��6;   $
��10,0q�6R   5      5       �� 5#   
   $��2��6   $��Center=�6"   $��5,0,5,0q�6=   $��2��6N   .+�5$      �� $��Chaîne chiffrée :��6   $��Center=�60   $��30(�6M   �� 5%      -   5         ��Name$ Resultat_Serveur��5%      ���	#FFEEEEEE$��Stretch=�6I   $��14(�6g   $��	5,10,5,10q�6u   $
��Wrap=�6�   5&      5'      5(      