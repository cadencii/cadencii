package org.kbinani.cadencii;

import java.awt.Dimension;
import java.awt.GridBagConstraints;
import java.awt.GridBagLayout;
import java.awt.Insets;
import javax.swing.JButton;
import javax.swing.JLabel;
import javax.swing.JTextArea;
import javax.swing.SwingConstants;
import org.kbinani.windows.forms.BDialog;
import org.kbinani.windows.forms.BDialogResult;
import java.awt.event.ActionListener;
import java.awt.event.ActionEvent;
import java.awt.Dialog.ModalExclusionType;
import java.awt.Dialog.ModalityType;

public class ExceptionNotifyFormUiImpl extends BDialog implements ExceptionNotifyFormUi
{
    private static final long serialVersionUID = -3483441400468011315L;
    private final ExceptionNotifyFormUiListener uiListener;
    private JLabel labelDescription;
    private JTextArea textMessage;
    private final JButton buttonCancel = new JButton();
    private JButton buttonSend;
    private JLabel lblNewLabel_1;
    
    public ExceptionNotifyFormUiImpl( ExceptionNotifyFormUiListener listener )
    {
        setModal(true);
        setResizable(false);
        setModalityType(ModalityType.APPLICATION_MODAL);
        setModalExclusionType(ModalExclusionType.APPLICATION_EXCLUDE);
        this.uiListener = listener;
        GridBagLayout gridBagLayout = new GridBagLayout();
        gridBagLayout.columnWidths = new int[]{0, 0, 0};
        gridBagLayout.columnWeights = new double[]{0.0, 1.0, 0.0};
        gridBagLayout.rowHeights = new int[]{0, 0, 0};
        gridBagLayout.rowWeights = new double[]{0.0, 1.0, 0.0};
        getContentPane().setLayout(gridBagLayout);
        
        this.labelDescription = new JLabel("New label");
        this.labelDescription.setHorizontalAlignment(SwingConstants.RIGHT);
        GridBagConstraints gbc_labelDescription = new GridBagConstraints();
        gbc_labelDescription.anchor = GridBagConstraints.WEST;
        gbc_labelDescription.gridwidth = 3;
        gbc_labelDescription.insets = new Insets(30, 22, 5, 22);
        gbc_labelDescription.gridx = 0;
        gbc_labelDescription.gridy = 0;
        getContentPane().add(this.labelDescription, gbc_labelDescription);
        GridBagConstraints gbc_buttonCancel = new GridBagConstraints();
        gbc_buttonCancel.insets = new Insets(22, 22, 30, 5);
        gbc_buttonCancel.gridx = 0;
        gbc_buttonCancel.gridy = 2;
        getContentPane().add(buttonCancel, gbc_buttonCancel);
        buttonCancel.setText("Cancel");
        buttonCancel.setPreferredSize(new Dimension(100, 29));
        
        this.textMessage = new JTextArea();
        GridBagConstraints gbc_textMessage = new GridBagConstraints();
        gbc_textMessage.gridwidth = 3;
        gbc_textMessage.insets = new Insets(0, 22, 5, 22);
        gbc_textMessage.ipady = 1;
        gbc_textMessage.fill = GridBagConstraints.BOTH;
        gbc_textMessage.gridx = 0;
        gbc_textMessage.gridy = 1;
        getContentPane().add(this.textMessage, gbc_textMessage);
        
        this.lblNewLabel_1 = new JLabel(" ");
        GridBagConstraints gbc_lblNewLabel_1 = new GridBagConstraints();
        gbc_lblNewLabel_1.insets = new Insets(0, 0, 0, 5);
        gbc_lblNewLabel_1.gridx = 1;
        gbc_lblNewLabel_1.gridy = 2;
        getContentPane().add(this.lblNewLabel_1, gbc_lblNewLabel_1);
        
        this.buttonSend = new JButton();
        this.buttonSend.addActionListener(new ActionListener() {
            public void actionPerformed(ActionEvent e) {
                uiListener.sendButtonClick();
            }
        });
        this.buttonSend.setText("Send");
        this.buttonSend.setPreferredSize(new Dimension(150, 29));
        GridBagConstraints gbc_buttonSend = new GridBagConstraints();
        gbc_buttonSend.insets = new Insets(22, 5, 30, 22);
        gbc_buttonSend.gridx = 2;
        gbc_buttonSend.gridy = 2;
        getContentPane().add(this.buttonSend, gbc_buttonSend);
    }

    @Override
    public int showDialog(
        Object parent_form )
    {
        this.setSize( 379, 421 );
        BDialogResult ret = BDialogResult.CANCEL;
        if( parent_form == null || (parent_form != null && !(parent_form instanceof BDialog)) ){
            ret = super.showDialog( null );
        }else{
            BDialog form = (BDialog)parent_form;
            ret = super.showDialog( form );
        }
        if( ret == BDialogResult.OK || ret == BDialogResult.YES ){
            return 1;
        }else{
            return 0;
        }
    }

    @Override
    public void setDescription(
        String value )
    {
        this.labelDescription.setText( value );
    }

    @Override
    public void setExceptionMessage(
        String value )
    {
        this.textMessage.setText( value );
        
    }

    @Override
    public void setCancelButtonText(
        String value )
    {
        this.buttonCancel.setText( value );
    }

    @Override
    public void setSendButtonText(
        String value )
    {
        this.buttonSend.setText( value );
    }
}
