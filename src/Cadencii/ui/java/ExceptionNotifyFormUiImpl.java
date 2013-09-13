package com.github.cadencii.ui;

import java.awt.Dimension;
import java.awt.GridBagConstraints;
import java.awt.GridBagLayout;
import java.awt.Insets;
import java.awt.event.ActionEvent;
import java.awt.event.ActionListener;
import javax.swing.JButton;
import javax.swing.JLabel;
import javax.swing.JTextArea;
import javax.swing.SwingConstants;

import com.github.cadencii.ExceptionNotifyFormUi;
import com.github.cadencii.ExceptionNotifyFormUiListener;
import javax.swing.JScrollPane;

public class ExceptionNotifyFormUiImpl extends DialogBase implements ExceptionNotifyFormUi
{
    private static final long serialVersionUID = -3483441400468011315L;
    private final ExceptionNotifyFormUiListener uiListener;
    private JLabel labelDescription;
    private JTextArea textMessage;
    private final JButton buttonCancel = new JButton();
    private JButton buttonSend;
    private JScrollPane scrollPane;
    private JLabel lblNewLabel;

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
        buttonCancel.addActionListener(new ActionListener() {
            public void actionPerformed(ActionEvent arg0) {
                uiListener.cancelButtonClick();
            }
        });

        this.scrollPane = new JScrollPane();
        GridBagConstraints gbc_scrollPane = new GridBagConstraints();
        gbc_scrollPane.gridwidth = 3;
        gbc_scrollPane.weighty = 1.0;
        gbc_scrollPane.weightx = 1.0;
        gbc_scrollPane.insets = new Insets(0, 22, 5, 22);
        gbc_scrollPane.fill = GridBagConstraints.BOTH;
        gbc_scrollPane.gridx = 0;
        gbc_scrollPane.gridy = 1;
        getContentPane().add(this.scrollPane, gbc_scrollPane);

        this.textMessage = new JTextArea();
        this.scrollPane.setViewportView(this.textMessage);
        getContentPane().add(buttonCancel, gbc_buttonCancel);
        buttonCancel.setText("Cancel");
        buttonCancel.setPreferredSize(new Dimension(100, 29));

        this.buttonSend = new JButton();
        this.buttonSend.addActionListener(new ActionListener() {
            public void actionPerformed(ActionEvent e) {
                uiListener.sendButtonClick();
            }
        });

        this.lblNewLabel = new JLabel(" ");
        GridBagConstraints gbc_lblNewLabel = new GridBagConstraints();
        gbc_lblNewLabel.fill = GridBagConstraints.HORIZONTAL;
        gbc_lblNewLabel.weightx = 1.0;
        gbc_lblNewLabel.insets = new Insets(0, 0, 0, 5);
        gbc_lblNewLabel.gridx = 1;
        gbc_lblNewLabel.gridy = 2;
        getContentPane().add(this.lblNewLabel, gbc_lblNewLabel);
        this.buttonSend.setText("Send");
        this.buttonSend.setPreferredSize(new Dimension(150, 29));
        GridBagConstraints gbc_buttonSend = new GridBagConstraints();
        gbc_buttonSend.insets = new Insets(22, 5, 30, 22);
        gbc_buttonSend.gridx = 2;
        gbc_buttonSend.gridy = 2;
        getContentPane().add(this.buttonSend, gbc_buttonSend);
    }

    public int showDialog( Object parent_form )
    {
        this.setSize( 379, 421 );
        return super.doShowDialog( parent_form );
    }

    public void setDescription(
        String value )
    {
        this.labelDescription.setText( value );
    }

    public void setExceptionMessage(
        String value )
    {
        this.textMessage.setText( value );
    }

    public void setCancelButtonText(
        String value )
    {
        this.buttonCancel.setText( value );
    }

    public void setSendButtonText(
        String value )
    {
        this.buttonSend.setText( value );
    }

    @Override
    public void close()
    {
        super.doClose();
    }
}
