using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using UMIDLibrary;

namespace TestUMIDSAM
{
    public partial class Form1 : Form
    {
        UMIDLibrary.AllCardTech_PCSC pcsc;
        UMIDLibrary.AllCardTech_Smart_Card sc;
        UMIDLibrary.AllCardTech_Util util = new UMIDLibrary.AllCardTech_Util();

        public Form1()
        {
            InitializeComponent();
            setDefaultValues();
            sc = new UMIDLibrary.AllCardTech_Smart_Card();
            pcsc = new UMIDLibrary.AllCardTech_PCSC();
            sc.InitializeReaders();
            foreach (String s in sc.ReaderList)
            {
                if (s != null)
                {
                    this.cbReader.Items.Add(s);
                    this.cbSAM.Items.Add(s);
                }
            }

            if (this.cbSAM.Items.Count == 0)
            {
                MessageBox.Show("No Cardreader device detected, app closing", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Application.Exit();
                this.Close();
                return;
            }


            this.cbSAM.SelectedIndex = 0;
            this.cbReader.SelectedIndex = 1;
            this.cbTestCase.Items.Add("Select Applet");
            this.cbTestCase.Items.Add("Get Status");
            this.cbTestCase.Items.Add("Activate UMID Card");
            this.cbTestCase.Items.Add("Change PIN");
            this.cbTestCase.Items.Add("Authenticate SL1");
            this.cbTestCase.Items.Add("Authenticate SL2");
            this.cbTestCase.Items.Add("Authenticate SL3");
            this.cbTestCase.Items.Add("Read CRN");
            this.cbTestCase.Items.Add("Read CSN");
            this.cbTestCase.Items.Add("Card Creation Date");
            this.cbTestCase.Items.Add("Birthday");
            this.cbTestCase.Items.Add("First Name");
            this.cbTestCase.Items.Add("Middle Name");
            this.cbTestCase.Items.Add("Last Name");
            this.cbTestCase.Items.Add("PLACE_OF_BIRTH_COUNTRY");
            this.cbTestCase.Items.Add("WEIGHT");
            this.cbTestCase.Items.Add("HEIGHT");
            this.cbTestCase.Items.Add("DISTINGUISHING FEATURE");
            this.cbTestCase.Items.Add("MARITAL STATUS");
            this.cbTestCase.Items.Add("PHOTO");
            this.cbTestCase.Items.Add("SIGNATURE");
            this.cbTestCase.Items.Add("Fingerprint Left Primary Code");
            this.cbTestCase.Items.Add("Fingerprint Left Secondary Code");
            this.cbTestCase.Items.Add("Fingerprint Right Primary Code");
            this.cbTestCase.Items.Add("Fingerprint Right Secondary Code");
            this.cbTestCase.Items.Add("Fingerprint Left Primary");
            this.cbTestCase.Items.Add("Fingerprint Left Secondary");
            this.cbTestCase.Items.Add("Fingerprint Right Primary");
            this.cbTestCase.Items.Add("Fingerprint Right Secondary");
            this.cbTestCase.Items.Add("Update Marital Status to 1");
            this.cbTestCase.Items.Add("Update Height to 102 and Weight to 48");
            this.cbTestCase.Items.Add("Update Distinguishing Features to Mole in left cheek");
            this.cbTestCase.Items.Add("Read Agency Data");
            this.cbTestCase.Items.Add("Update Agency Data");
            this.cbTestCase.Items.Add("Block Card");
            this.cbTestCase.Items.Add("Delete Instance");


            this.cbTestCase.Items.Add("Read Reserve 1");
            this.cbTestCase.Items.Add("Write Reserve 1");
            this.cbTestCase.Items.Add("Rewrite Fingerprint data");

            this.cbTestCase.Items.Add("Read BPNO");
            this.cbTestCase.Items.Add("Read GSISNO");

            this.cbTestCase.SelectedIndex = 0;
        }

        private void setDefaultValues()
        {
            this.txtPhotoPath.Text = Application.StartupPath + "\\UMID_Photo.jpg";
            this.txtSignature.Text = Application.StartupPath + "\\UMID_Signature.tiff";
            this.txtPrimaryLeft.Text = Application.StartupPath + "\\lp.ansi-fmr";
            this.txtSecondaryLeft.Text = Application.StartupPath + "\\ls.ansi-fmr";
            this.txtPrimaryRight.Text = Application.StartupPath + "\\rp.ansi-fmr";
            this.txtSecondaryRight.Text = Application.StartupPath + "\\rs.ansi-fmr";
        }

        private void button1_Click(object sender, EventArgs e)
        {
            executeTest(this.cbTestCase.Text);
        }

        private void executeTest(String test)
        {
            switch (test)
            {
                case "Select Applet":
                    setSL(false, false, false);
                    lstSelect.Items.Add(sc.SelectApplet(this.cbReader.SelectedIndex, this.cbSAM.SelectedIndex).ToString());
                    break;
                case "Get Status":
                    String status = "";
                    sc.GetCardStatus(ref status);
                    MessageBox.Show(status);
                    break;
                case "Activate UMID Card":
                    sc.UMIDCard_Activate(util.AsciiToByteArray(this.txtUserPin.Text), this.txtUserPin.Text.Length).ToString();
                    break;
                case "Change PIN":
                    sc.UMIDCard_Change_PIN(this.txtOlPin.Text, this.txtUserPin.Text);
                    break;
                case "Authenticate SL1":
                    setSL(sc.AuthenticateSL1(), null, null);
                    break;
                case "Authenticate SL2":
                    setSL(null, sc.AuthenticateSL2(util.AsciiToByteArray(this.txtUserPin.Text)), null);
                    break;
                case "Authenticate SL3":
                    setSL(null, null, sc.AuthenticateSL3());
                    break;
                case "Read CRN":
                    this.lblCRN.Text = util.ByteArrayToAscii(sc.get_getUmidData(UMIDLibrary.AllCardTech_Smart_Card.UMID_Fields.CRN));
                    break;
                case "Read CSN":
                    this.lblCSN.Text = util.ByteArrayToAscii(sc.get_getUmidData(UMIDLibrary.AllCardTech_Smart_Card.UMID_Fields.CSN));
                    break;
                case "Card Creation Date":
                    this.lblCardCreationDate.Text = util.ByteArrayToAscii(sc.get_getUmidData(UMIDLibrary.AllCardTech_Smart_Card.UMID_Fields.CARD_CREATION_DATE));
                    break;
                case "Birthday":
                    this.lblBday.Text = util.ByteArrayToAscii(sc.get_getUmidData(UMIDLibrary.AllCardTech_Smart_Card.UMID_Fields.DATE_OF_BIRTH));
                    break;
                case "First Name":
                    this.lblFName.Text = util.ByteArrayToAscii(sc.get_getUmidData(UMIDLibrary.AllCardTech_Smart_Card.UMID_Fields.FIRST_NAME));
                    break;
                case "Middle Name":
                    this.lblMName.Text = util.ByteArrayToAscii(sc.get_getUmidData(UMIDLibrary.AllCardTech_Smart_Card.UMID_Fields.MIDDLE_NAME));
                    break;
                case "Last Name":
                    this.lblLName.Text = util.ByteArrayToAscii(sc.get_getUmidData(UMIDLibrary.AllCardTech_Smart_Card.UMID_Fields.LAST_NAME));
                    break;
                case "PLACE_OF_BIRTH_COUNTRY":
                    MessageBox.Show(util.ByteArrayToHexString(sc.get_getUmidData(UMIDLibrary.AllCardTech_Smart_Card.UMID_Fields.PLACE_OF_BIRTH_COUNTRY)));
                    break;
                case "WEIGHT":
                    MessageBox.Show(util.ByteArrayToHexString(sc.get_getUmidData(UMIDLibrary.AllCardTech_Smart_Card.UMID_Fields.WEIGHT)));
                    break;
                case "HEIGHT":
                    MessageBox.Show(util.ByteArrayToHexString(sc.get_getUmidData(UMIDLibrary.AllCardTech_Smart_Card.UMID_Fields.HEIGHT)));
                    break;
                case "DISTINGUISHING FEATURE":
                    MessageBox.Show(util.ByteArrayToHexString(sc.get_getUmidData(UMIDLibrary.AllCardTech_Smart_Card.UMID_Fields.DISTINGUISHING_FEATURES)));
                    break;
                case "MARITAL STATUS":
                    MessageBox.Show(util.ByteArrayToHexString(sc.get_getUmidData(UMIDLibrary.AllCardTech_Smart_Card.UMID_Fields.MARITAL_STATUS)));
                    break;
                case "PHOTO":
                    if (txtPhotoPath.Text == "")
                    {
                        MessageBox.Show("Invalid Path");
                    }
                    if (sc.getUmidFile(txtPhotoPath.Text,
                        UMIDLibrary.AllCardTech_Smart_Card.UMID_Fields.BIOMETRIC_PICTURE))
                    {
                        this.picPhoto.Image = Image.FromFile(txtPhotoPath.Text);
                    }
                    else
                    {
                        this.picPhoto.Image = null;
                    }
                    break;
                case "SIGNATURE":
                    if (txtSignature.Text == "")
                    {
                        MessageBox.Show("Invalid Path");
                    }
                    if (sc.getUmidFile(txtSignature.Text,
                        UMIDLibrary.AllCardTech_Smart_Card.UMID_Fields.BIOMETRIC_SIGNATURE))
                    {
                        this.picSignature.Image = Image.FromFile(txtSignature.Text);
                    }
                    else
                    {
                        this.picSignature.Image = null;
                    }
                    break;
                case "Fingerprint Left Primary Code":
                    this.lblPrimaryLeft.Text = util.ByteArrayToAscii(sc.get_getUmidData(UMIDLibrary.AllCardTech_Smart_Card.UMID_Fields.LEFT_PRIMARY_FINGER_CODE));
                    break;
                case "Fingerprint Left Secondary Code":
                    this.lblSecondaryLeft.Text = util.ByteArrayToAscii(sc.get_getUmidData(UMIDLibrary.AllCardTech_Smart_Card.UMID_Fields.LEFT_SECONDARY_FINGER_CODE));
                    break;
                case "Fingerprint Right Primary Code":
                    this.lblPrimaryRight.Text = util.ByteArrayToAscii(sc.get_getUmidData(UMIDLibrary.AllCardTech_Smart_Card.UMID_Fields.RIGHT_PRIMARY_FINGER_CODE));
                    break;
                case "Fingerprint Right Secondary Code":
                    this.lblSecondaryRight.Text = util.ByteArrayToAscii(sc.get_getUmidData(UMIDLibrary.AllCardTech_Smart_Card.UMID_Fields.RIGHT_SECONDARY_FINGER_CODE));
                    break;
                case "Fingerprint Left Primary":
                    MessageBox.Show(sc.getUmidFile(txtPrimaryLeft.Text, UMIDLibrary.AllCardTech_Smart_Card.UMID_Fields.BIOMETRIC_LEFT_PRIMARY_FINGER).ToString());
                    break;
                case "Fingerprint Left Secondary":
                    MessageBox.Show(sc.getUmidFile(txtSecondaryLeft.Text, UMIDLibrary.AllCardTech_Smart_Card.UMID_Fields.BIOMETRIC_LEFT_SECONDARY_FINGER).ToString());
                    break;
                case "Fingerprint Right Primary":
                    MessageBox.Show(sc.getUmidFile(txtPrimaryRight.Text, UMIDLibrary.AllCardTech_Smart_Card.UMID_Fields.BIOMETRIC_RIGHT_PRIMARY_FINGER).ToString());
                    break;
                case "Fingerprint Right Secondary":
                    MessageBox.Show(sc.getUmidFile(txtSecondaryRight.Text, UMIDLibrary.AllCardTech_Smart_Card.UMID_Fields.BIOMETRIC_RIGHT_SECONDARY_FINGER).ToString());
                    break;
                case "Update Marital Status to 1":
                    MessageBox.Show(sc.WriteMartialStatus("1").ToString());
                    break;
                case "Update Height to 102 and Weight to 48":
                    MessageBox.Show(sc.WriteHeightWeight(102, 48).ToString());
                    break;
                case "Update Distinguishing Features to Mole in left cheek":
                    MessageBox.Show(sc.WriteDistinguishing("Mole in left cheek").ToString());
                    break;
                case "Read Agency Data":
                    readAgency();
                    break;
                case "Update Agency Data":
                    updateAgency();
                    break;
                case "Block Card":
                    MessageBox.Show(sc.ApplicationBlock().ToString());
                    break;
                case "Delete Instance":
                    MessageBox.Show(sc.DeleteInstance().ToString());
                    break;
                case "Read Reserve 1":
                    readReserve1();
                    break;
                case "Write Reserve 1":
                    writeReserve1();
                    break;
                case "Rewrite Fingerprint data":
                    MessageBox.Show(sc.RewriteFingerprintLeftPrimary("ACC", "10001", System.IO.File.ReadAllBytes("lp.ansi-fmr")).ToString());
                    MessageBox.Show(sc.RewriteFingerprintLeftSecondary("ACC", "10001", System.IO.File.ReadAllBytes("ls.ansi-fmr")).ToString());
                    MessageBox.Show(sc.RewriteFingerprintRightPrimary("ACC", "10001", System.IO.File.ReadAllBytes("rp.ansi-fmr")).ToString());
                    MessageBox.Show(sc.RewriteFingerprintRightSecondary("ACC", "10001", System.IO.File.ReadAllBytes("rs.ansi-fmr")).ToString());
                    break;
                case "Read BPNO":
                    if (sc.SelectApplet(this.cbReader.SelectedIndex, this.cbSAM.SelectedIndex))
                    {
                        if (sc.AuthenticateSL1())
                        {
                            MessageBox.Show("BP 46-" + util.ByteArrayToAscii(sc.get_getGSISData(UMIDLibrary.AllCardTech_Smart_Card.GSIS_FIELDS.GSIS_11, 0, 10)));
                        }
                    }
                    break;
                case "Read GSISNO":
                    if (sc.SelectApplet(this.cbReader.SelectedIndex, this.cbSAM.SelectedIndex))
                    {
                        if (sc.AuthenticateSL1())
                        {
                            MessageBox.Show("GSIS 47-" + util.ByteArrayToAscii(sc.get_getGSISData(UMIDLibrary.AllCardTech_Smart_Card.GSIS_FIELDS.GSIS_12, 0, 11)));
                        }
                    }
                    break;
                default:
                    MessageBox.Show("No Test Case Selected");
                    break;
            }
        }

        private void readAgency()
        {
            int sector = int.Parse(this.txtElementNo.Text);
            int offset = int.Parse(this.txtOff.Text);
            int len = int.Parse(this.txtLen.Text);
            if (cbAgency.SelectedIndex == 1)
            {
                sector += 36;
            }
            else if (cbAgency.SelectedIndex == 2)
            {
                sector += 72;
            }
            else if (cbAgency.SelectedIndex == 3)
            {
                sector += 108;
            }
            this.txtData.Text = new UMIDLibrary.AllCardTech_Util().ByteArrayToAscii(sc.ReadSector(sector, offset, len));
        }

        private void updateAgency()
        {
            int sector = int.Parse(this.txtElementNo.Text);
            int offset = int.Parse(this.txtOff.Text);
            int len = this.txtData.Text.Length;
            var util = new UMIDLibrary.AllCardTech_Util();
            if (cbAgency.SelectedIndex == 1)
            {
                sector += 36;
            }
            else if (cbAgency.SelectedIndex == 2)
            {
                sector += 72;
            }
            else if (cbAgency.SelectedIndex == 3)
            {
                sector += 108;
            }
            sc.WriteSector(sector, offset, len, util.AsciiToByteArray(this.txtData.Text));
        }

        private void readReserve1()
        {
            /*int offset = int.Parse(this.txtOff.Text);
            int len = int.Parse(this.txtLen.Text);
             this.txtData.Text = new UMIDLibrary.AllCardTech_Util().ByteArrayToAscii(sc.ReadReserve1(offset, len));*/
            String initials = "";
            DateTime date = new DateTime();
            String terminalId = "";
            sc.ReadLatestUMIDRewritingLog(ref initials, ref date, ref terminalId);
            MessageBox.Show(initials + " " + date + " " + terminalId);

        }

        private void writeReserve1()
        {
            /*int offset = int.Parse(this.txtOff.Text);
            int len = this.txtData.Text.Length;
            var util = new UMIDLibrary.AllCardTech_Util();
          sc.WriteReserve1(offset, len, util.AsciiToByteArray(this.txtData.Text));
             * */
            //sc.LogLatestUMIDRewriting("ABC", "10000001");
        }

        private void setSL(bool? sl1, bool? sl2, bool? sl3)
        {
            if ((sl1 != null && sl2 != null && sl3 != null) && !(sl1.Value && sl2.Value && sl3.Value))
            {
                foreach (Object o in this.Controls)
                {
                    if (o is TextBox)
                    {
                        ((TextBox)o).Enabled = false;
                    }
                    if (o is Label)
                    {
                        if (((Label)o).Tag != null && ((Label)o).Tag.Equals("data"))
                            ((Label)o).Text = "";
                    }
                }
                this.picPhoto.Image = null;
            }
            if (sl1 != null)
                this.lblSL1.Text = sl1.Value ? "PASSED" : "FAILED";
            if (sl2 != null)
                this.lblSL2.Text = sl2.Value ? "PASSED" : "FAILED";
            if (sl3 != null)
                this.lblSL3.Text = sl3.Value ? "PASSED" : "FAILED";
        }

        private void cbTestCase_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (cbTestCase.Text)
            {
                case "Select Applet":
                    break;
                case "Activate UMID Card":
                    break;
                case "Authenticate SL1":
                    break;
                case "Authenticate SL2":
                    this.txtUserPin.Enabled = true;
                    break;
                case "Authenticate SL3":
                    break;
                case "Read CRN":
                    break;
                case "Read CSN":
                    break;
                case "Birthday":
                    break;
                case "First Name":
                    break;
                case "Last Name":
                    break;
                case "PLACE_OF_BIRTH_COUNTRY":
                    break;
                case "WEIGHT":
                    break;
                case "MARITAL STATUS":
                    break;
                case "PHOTO":
                    txtPhotoPath.Enabled = true;
                    break;
                case "Fingerprint Left Primary Code":
                    break;
                case "Fingerprint Left Secondary Code":
                    break;
                case "Fingerprint Right Primary Code":
                    break;
                case "Fingerprint Right Secondary Code":
                    break;
                case "Fingerprint Left Primary":
                    break;
                case "Fingerprint Left Secondary":
                    break;
                case "Fingerprint Right Primary":
                    break;
                case "Fingerprint Right Secondary":
                    break;
                case "Read Agency Data":
                case "Update Agency Data":
                    this.txtData.Enabled = true;
                    this.txtElementNo.Enabled = true;
                    this.txtOff.Enabled = true;
                    this.txtLen.Enabled = true;
                    break;
                default:
                    break;
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            //tabControl1.TabPages.RemoveAt(2);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                DateTime start = DateTime.Now;

                sc.DisconnectApplet();
                if (sc.SelectApplet(this.cbReader.SelectedIndex, this.cbSAM.SelectedIndex) && sc.AuthenticateSL1())
                {

                    String CRN = "CRN-" + util.ByteArrayToAscii(sc.get_getUmidData(UMIDLibrary.AllCardTech_Smart_Card.UMID_Fields.CRN));
                    String CSN = "CSN-" + util.ByteArrayToAscii(sc.get_getUmidData(UMIDLibrary.AllCardTech_Smart_Card.UMID_Fields.CSN));
                    String CreationDate = "Creation Date-" + util.ByteArrayToAscii(sc.get_getUmidData(UMIDLibrary.AllCardTech_Smart_Card.UMID_Fields.CARD_CREATION_DATE));

                    this.richTextBox1.Text = CRN + "\r\n" + CSN + "\r\n" + CreationDate;

                    this.lstShareData.Items.Add(CRN);
                    this.lstShareData.Items.Add(CSN);
                    this.lstShareData.Items.Add(CreationDate);

                    this.lstShareData.Items.Add("First Name-" + util.ByteArrayToAscii(sc.get_getUmidData(UMIDLibrary.AllCardTech_Smart_Card.UMID_Fields.FIRST_NAME)));
                    this.lstShareData.Items.Add("Middle Name-" + util.ByteArrayToAscii(sc.get_getUmidData(UMIDLibrary.AllCardTech_Smart_Card.UMID_Fields.MIDDLE_NAME)));
                    this.lstShareData.Items.Add("Last Name-" + util.ByteArrayToAscii(sc.get_getUmidData(UMIDLibrary.AllCardTech_Smart_Card.UMID_Fields.LAST_NAME)));
                    this.lstShareData.Items.Add("Suffix-" + util.ByteArrayToAscii(sc.get_getUmidData(UMIDLibrary.AllCardTech_Smart_Card.UMID_Fields.SUFFIX)));
                    this.lstShareData.Items.Add("Address Postal Code-" + util.ByteArrayToAscii(sc.get_getUmidData(UMIDLibrary.AllCardTech_Smart_Card.UMID_Fields.ADDRESS_POSTAL_CODE)));
                    this.lstShareData.Items.Add("Address Country Code-" + util.ByteArrayToAscii(sc.get_getUmidData(UMIDLibrary.AllCardTech_Smart_Card.UMID_Fields.ADDRESS_COUNTRY)));
                    this.lstShareData.Items.Add("Address Province-" + util.ByteArrayToAscii(sc.get_getUmidData(UMIDLibrary.AllCardTech_Smart_Card.UMID_Fields.ADDRESS_PROVINCIAL_OR_STATE)));
                    this.lstShareData.Items.Add("Address City-" + util.ByteArrayToAscii(sc.get_getUmidData(UMIDLibrary.AllCardTech_Smart_Card.UMID_Fields.ADDRESS_CITY_OR_MUNICIPALITY)));
                    this.lstShareData.Items.Add("Address Barangay-" + util.ByteArrayToAscii(sc.get_getUmidData(UMIDLibrary.AllCardTech_Smart_Card.UMID_Fields.ADDRESS_BARANGAY_OR_DISTRIC_OR_LOCALITY)));
                    this.lstShareData.Items.Add("Address Subdivision-" + util.ByteArrayToAscii(sc.get_getUmidData(UMIDLibrary.AllCardTech_Smart_Card.UMID_Fields.ADDRESS_SUBDIVISION)));
                    this.lstShareData.Items.Add("Address Street-" + util.ByteArrayToAscii(sc.get_getUmidData(UMIDLibrary.AllCardTech_Smart_Card.UMID_Fields.ADDRESS_STREET_NAME)));
                    this.lstShareData.Items.Add("Address House/Block No-" + util.ByteArrayToAscii(sc.get_getUmidData(UMIDLibrary.AllCardTech_Smart_Card.UMID_Fields.ADDRESS_HOUSE_OR_LOT_AND_BLOCK_NUMBER)));
                    this.lstShareData.Items.Add("Address Rm/Flr/Unit/Bldg-" + util.ByteArrayToAscii(sc.get_getUmidData(UMIDLibrary.AllCardTech_Smart_Card.UMID_Fields.ADDRESS_ROOM_OR_FLOOR_OR_UNIT_NO_AND_BUILDING_NAME)));
                    this.lstShareData.Items.Add("Gender-" + util.ByteArrayToAscii(sc.get_getUmidData(UMIDLibrary.AllCardTech_Smart_Card.UMID_Fields.GENDER)));
                    this.lstShareData.Items.Add("Birthday-" + util.ByteArrayToAscii(sc.get_getUmidData(UMIDLibrary.AllCardTech_Smart_Card.UMID_Fields.DATE_OF_BIRTH)));
                    this.lstShareData.Items.Add("Left Primary Code-" + util.ByteArrayToAscii(sc.get_getUmidData(UMIDLibrary.AllCardTech_Smart_Card.UMID_Fields.LEFT_PRIMARY_FINGER_CODE)));
                    this.lstShareData.Items.Add("Right Primary Code-" + util.ByteArrayToAscii(sc.get_getUmidData(UMIDLibrary.AllCardTech_Smart_Card.UMID_Fields.RIGHT_PRIMARY_FINGER_CODE)));
                    this.lstShareData.Items.Add("Left Secondary Code-" + util.ByteArrayToAscii(sc.get_getUmidData(UMIDLibrary.AllCardTech_Smart_Card.UMID_Fields.LEFT_SECONDARY_FINGER_CODE)));
                    this.lstShareData.Items.Add("Right Secondary Code-" + util.ByteArrayToAscii(sc.get_getUmidData(UMIDLibrary.AllCardTech_Smart_Card.UMID_Fields.RIGHT_SECONDARY_FINGER_CODE)));
                    this.lstShareData.Items.Add("Left Primary-" + sc.getUmidFile(Application.StartupPath + "\\lp.ansi-fmr", UMIDLibrary.AllCardTech_Smart_Card.UMID_Fields.BIOMETRIC_LEFT_PRIMARY_FINGER));
                    this.lstShareData.Items.Add("Right Primary-" + sc.getUmidFile(Application.StartupPath + "\\rp.ansi-fmr", UMIDLibrary.AllCardTech_Smart_Card.UMID_Fields.BIOMETRIC_RIGHT_PRIMARY_FINGER));
                    this.lstShareData.Items.Add("Left Secondary-" + sc.getUmidFile(Application.StartupPath + "\\ls.ansi-fmr", UMIDLibrary.AllCardTech_Smart_Card.UMID_Fields.BIOMETRIC_LEFT_SECONDARY_FINGER));
                    this.lstShareData.Items.Add("Right Secondary-" + sc.getUmidFile(Application.StartupPath + "\\rs.ansi-fmr", UMIDLibrary.AllCardTech_Smart_Card.UMID_Fields.BIOMETRIC_RIGHT_SECONDARY_FINGER));
                    this.lstShareData.Items.Add("Picture-" + sc.getUmidFile(Application.StartupPath + "\\UMID_Photo.jpg", UMIDLibrary.AllCardTech_Smart_Card.UMID_Fields.BIOMETRIC_PICTURE));
                    this.lstShareData.Items.Add("Height-" + util.ByteArrayToAscii(sc.get_getUmidData(UMIDLibrary.AllCardTech_Smart_Card.UMID_Fields.HEIGHT)));
                    this.lstShareData.Items.Add("Weight-" + util.ByteArrayToAscii(sc.get_getUmidData(UMIDLibrary.AllCardTech_Smart_Card.UMID_Fields.WEIGHT)));
                    this.lstShareData.Items.Add("Distinguishing Features-" + util.ByteArrayToAscii(sc.get_getUmidData(UMIDLibrary.AllCardTech_Smart_Card.UMID_Fields.DISTINGUISHING_FEATURES)));
                    if (sc.AuthenticateSL2(new byte[] { 0x31, 0x32, 0x33, 0x34, 0x35, 0x36, 0x37, 0x38 }))
                    {
                        this.lstShareData.Items.Add("Marital Status-" + util.ByteArrayToAscii(sc.get_getUmidData(UMIDLibrary.AllCardTech_Smart_Card.UMID_Fields.MARITAL_STATUS)));
                        this.lstShareData.Items.Add("Signature-" + sc.getUmidFile(Application.StartupPath + "\\sign.tiff", UMIDLibrary.AllCardTech_Smart_Card.UMID_Fields.BIOMETRIC_SIGNATURE));
                        if (sc.AuthenticateSL3())
                        {
                            this.lstShareData.Items.Add("Place of Birth- City-" + util.ByteArrayToAscii(sc.get_getUmidData(UMIDLibrary.AllCardTech_Smart_Card.UMID_Fields.PLACE_OF_BIRTH_CITY)));
                            this.lstShareData.Items.Add("Place of Birth- Province-" + util.ByteArrayToAscii(sc.get_getUmidData(UMIDLibrary.AllCardTech_Smart_Card.UMID_Fields.PLACE_OF_BIRTH_PROVINCE)));
                            this.lstShareData.Items.Add("Place of Birth- Country-" + util.ByteArrayToAscii(sc.get_getUmidData(UMIDLibrary.AllCardTech_Smart_Card.UMID_Fields.PLACE_OF_BIRTH_COUNTRY)));
                            this.lstShareData.Items.Add("Place of Birth- Father First-" + util.ByteArrayToAscii(sc.get_getUmidData(UMIDLibrary.AllCardTech_Smart_Card.UMID_Fields.FATHER_FIRST_NAME)));
                            this.lstShareData.Items.Add("Place of Birth- Father Middle-" + util.ByteArrayToAscii(sc.get_getUmidData(UMIDLibrary.AllCardTech_Smart_Card.UMID_Fields.FATHER_MIDDLE_NAME)));
                            this.lstShareData.Items.Add("Place of Birth- Father Last-" + util.ByteArrayToAscii(sc.get_getUmidData(UMIDLibrary.AllCardTech_Smart_Card.UMID_Fields.FATHER_LAST_NAME)));
                            this.lstShareData.Items.Add("Place of Birth- Father Suffix-" + util.ByteArrayToAscii(sc.get_getUmidData(UMIDLibrary.AllCardTech_Smart_Card.UMID_Fields.FATHER_SUFFIX)));
                            this.lstShareData.Items.Add("Place of Birth- Mother First-" + util.ByteArrayToAscii(sc.get_getUmidData(UMIDLibrary.AllCardTech_Smart_Card.UMID_Fields.MOTHER_FIRST_NAME)));
                            this.lstShareData.Items.Add("Place of Birth- Mother Middle-" + util.ByteArrayToAscii(sc.get_getUmidData(UMIDLibrary.AllCardTech_Smart_Card.UMID_Fields.MOTHER_MIDDLE_NAME)));
                            this.lstShareData.Items.Add("Place of Birth- Mother Last-" + util.ByteArrayToAscii(sc.get_getUmidData(UMIDLibrary.AllCardTech_Smart_Card.UMID_Fields.MOTHER_LAST_NAME)));
                            this.lstShareData.Items.Add("Place of Birth- Mother Suffix-" + util.ByteArrayToAscii(sc.get_getUmidData(UMIDLibrary.AllCardTech_Smart_Card.UMID_Fields.MOTHER_SUFFIX)));
                            this.lstShareData.Items.Add("Place of Birth- TIN-" + util.ByteArrayToAscii(sc.get_getUmidData(UMIDLibrary.AllCardTech_Smart_Card.UMID_Fields.TIN)));
                        }
                    }
                }
                else
                {
                    MessageBox.Show("Could not select applet");
                }
                MessageBox.Show("Finish, span " + DateTime.Now.Subtract(start).TotalSeconds.ToString());
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            try
            {
                DateTime start = DateTime.Now;
                sc.DisconnectApplet();
                if (sc.SelectApplet(this.cbReader.SelectedIndex, this.cbSAM.SelectedIndex))
                {
                    if (sc.AuthenticateSL1())
                    {
                        this.lstSSS.Items.Add("SSS 1-" + util.ByteArrayToAscii(sc.get_getSSSData(UMIDLibrary.AllCardTech_Smart_Card.SSS_FIELDS.SSS_1, 0, 1024)));
                        this.lstSSS.Items.Add("SSS 2-" + util.ByteArrayToAscii(sc.get_getSSSData(UMIDLibrary.AllCardTech_Smart_Card.SSS_FIELDS.SSS_2, 0, 512)));
                        this.lstSSS.Items.Add("SSS 3-" + util.ByteArrayToAscii(sc.get_getSSSData(UMIDLibrary.AllCardTech_Smart_Card.SSS_FIELDS.SSS_3, 0, 256)));
                        this.lstSSS.Items.Add("SSS 4-" + util.ByteArrayToAscii(sc.get_getSSSData(UMIDLibrary.AllCardTech_Smart_Card.SSS_FIELDS.SSS_4, 0, 256)));
                        this.lstSSS.Items.Add("SSS 5-" + util.ByteArrayToAscii(sc.get_getSSSData(UMIDLibrary.AllCardTech_Smart_Card.SSS_FIELDS.SSS_5, 0, 256)));
                        this.lstSSS.Items.Add("SSS 6-" + util.ByteArrayToAscii(sc.get_getSSSData(UMIDLibrary.AllCardTech_Smart_Card.SSS_FIELDS.SSS_6, 0, 256)));
                        this.lstSSS.Items.Add("SSS 7-" + util.ByteArrayToAscii(sc.get_getSSSData(UMIDLibrary.AllCardTech_Smart_Card.SSS_FIELDS.SSS_7, 0, 128)));
                        this.lstSSS.Items.Add("SSS 8-" + util.ByteArrayToAscii(sc.get_getSSSData(UMIDLibrary.AllCardTech_Smart_Card.SSS_FIELDS.SSS_8, 0, 128)));
                        this.lstSSS.Items.Add("SSS 9-" + util.ByteArrayToAscii(sc.get_getSSSData(UMIDLibrary.AllCardTech_Smart_Card.SSS_FIELDS.SSS_9, 0, 128)));
                        this.lstSSS.Items.Add("SSS 10-" + util.ByteArrayToAscii(sc.get_getSSSData(UMIDLibrary.AllCardTech_Smart_Card.SSS_FIELDS.SSS_10, 0, 128)));
                        this.lstSSS.Items.Add("SSS 11-" + util.ByteArrayToAscii(sc.get_getSSSData(UMIDLibrary.AllCardTech_Smart_Card.SSS_FIELDS.SSS_11, 0, 128)));
                        this.lstSSS.Items.Add("SSS 12-" + util.ByteArrayToAscii(sc.get_getSSSData(UMIDLibrary.AllCardTech_Smart_Card.SSS_FIELDS.SSS_12, 0, 128)));
                        this.lstSSS.Items.Add("SSS 13-" + util.ByteArrayToAscii(sc.get_getSSSData(UMIDLibrary.AllCardTech_Smart_Card.SSS_FIELDS.SSS_13, 0, 128)));
                        this.lstSSS.Items.Add("SSS 14-" + util.ByteArrayToAscii(sc.get_getSSSData(UMIDLibrary.AllCardTech_Smart_Card.SSS_FIELDS.SSS_14, 0, 128)));
                        this.lstSSS.Items.Add("SSS 15-" + util.ByteArrayToAscii(sc.get_getSSSData(UMIDLibrary.AllCardTech_Smart_Card.SSS_FIELDS.SSS_15, 0, 128)));
                        this.lstSSS.Items.Add("SSS 16-" + util.ByteArrayToAscii(sc.get_getSSSData(UMIDLibrary.AllCardTech_Smart_Card.SSS_FIELDS.SSS_16, 0, 128)));
                        this.lstSSS.Items.Add("SSS 17-" + util.ByteArrayToAscii(sc.get_getSSSData(UMIDLibrary.AllCardTech_Smart_Card.SSS_FIELDS.SSS_17, 0, 64)));
                        this.lstSSS.Items.Add("SSS 18-" + util.ByteArrayToAscii(sc.get_getSSSData(UMIDLibrary.AllCardTech_Smart_Card.SSS_FIELDS.SSS_18, 0, 64)));
                        this.lstSSS.Items.Add("SSS 19-" + util.ByteArrayToAscii(sc.get_getSSSData(UMIDLibrary.AllCardTech_Smart_Card.SSS_FIELDS.SSS_19, 0, 64)));
                        this.lstSSS.Items.Add("SSS 20-" + util.ByteArrayToAscii(sc.get_getSSSData(UMIDLibrary.AllCardTech_Smart_Card.SSS_FIELDS.SSS_20, 0, 64)));
                        this.lstSSS.Items.Add("SSS 21-" + util.ByteArrayToAscii(sc.get_getSSSData(UMIDLibrary.AllCardTech_Smart_Card.SSS_FIELDS.SSS_21, 0, 64)));
                        this.lstSSS.Items.Add("SSS 22-" + util.ByteArrayToAscii(sc.get_getSSSData(UMIDLibrary.AllCardTech_Smart_Card.SSS_FIELDS.SSS_22, 0, 64)));
                        this.lstSSS.Items.Add("SSS 23-" + util.ByteArrayToAscii(sc.get_getSSSData(UMIDLibrary.AllCardTech_Smart_Card.SSS_FIELDS.SSS_23, 0, 64)));
                        this.lstSSS.Items.Add("SSS 24-" + util.ByteArrayToAscii(sc.get_getSSSData(UMIDLibrary.AllCardTech_Smart_Card.SSS_FIELDS.SSS_24, 0, 64)));
                        this.lstSSS.Items.Add("SSS 25-" + util.ByteArrayToAscii(sc.get_getSSSData(UMIDLibrary.AllCardTech_Smart_Card.SSS_FIELDS.SSS_25, 0, 64)));
                        this.lstSSS.Items.Add("SSS 26-" + util.ByteArrayToAscii(sc.get_getSSSData(UMIDLibrary.AllCardTech_Smart_Card.SSS_FIELDS.SSS_26, 0, 64)));
                        this.lstSSS.Items.Add("SSS 27-" + util.ByteArrayToAscii(sc.get_getSSSData(UMIDLibrary.AllCardTech_Smart_Card.SSS_FIELDS.SSS_27, 0, 64)));
                        this.lstSSS.Items.Add("SSS 28-" + util.ByteArrayToAscii(sc.get_getSSSData(UMIDLibrary.AllCardTech_Smart_Card.SSS_FIELDS.SSS_28, 0, 64)));
                        this.lstSSS.Items.Add("SSS 29-" + util.ByteArrayToAscii(sc.get_getSSSData(UMIDLibrary.AllCardTech_Smart_Card.SSS_FIELDS.SSS_29, 0, 64)));
                        this.lstSSS.Items.Add("SSS 30-" + util.ByteArrayToAscii(sc.get_getSSSData(UMIDLibrary.AllCardTech_Smart_Card.SSS_FIELDS.SSS_30, 0, 64)));
                        this.lstSSS.Items.Add("SSS 31-" + util.ByteArrayToAscii(sc.get_getSSSData(UMIDLibrary.AllCardTech_Smart_Card.SSS_FIELDS.SSS_31, 0, 64)));
                        this.lstSSS.Items.Add("SSS 32-" + util.ByteArrayToAscii(sc.get_getSSSData(UMIDLibrary.AllCardTech_Smart_Card.SSS_FIELDS.SSS_32, 0, 64)));
                        this.lstSSS.Items.Add("SSS 33-" + util.ByteArrayToAscii(sc.get_getSSSData(UMIDLibrary.AllCardTech_Smart_Card.SSS_FIELDS.SSS_33, 0, 64)));
                        this.lstSSS.Items.Add("SSS 34-" + util.ByteArrayToAscii(sc.get_getSSSData(UMIDLibrary.AllCardTech_Smart_Card.SSS_FIELDS.SSS_34, 0, 64)));
                        this.lstSSS.Items.Add("SSS 35-" + util.ByteArrayToAscii(sc.get_getSSSData(UMIDLibrary.AllCardTech_Smart_Card.SSS_FIELDS.SSS_35, 0, 64)));
                        this.lstSSS.Items.Add("SSS 36-" + util.ByteArrayToAscii(sc.get_getSSSData(UMIDLibrary.AllCardTech_Smart_Card.SSS_FIELDS.SSS_36, 0, 64)));

                    }
                    MessageBox.Show("Finish, span " + DateTime.Now.Subtract(start).TotalSeconds.ToString());
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            try
            {
                DateTime start = DateTime.Now;
                sc.DisconnectApplet();
                if (sc.SelectApplet(this.cbReader.SelectedIndex, this.cbSAM.SelectedIndex))
                {
                    if (sc.AuthenticateSL1())
                    {
                        String str = "";
                        for (int i = 0; i < 1024; i++)
                        {
                            str += "1";
                        }
                        sc.WriteSector(1, 0, 1024, util.AsciiToByteArray(str));

                        str = "";
                        for (int i = 0; i < 512; i++)
                        {
                            str += "2";
                        }
                        sc.WriteSector(2, 0, 512, util.AsciiToByteArray(str));

                        str = "";
                        for (int i = 0; i < 256; i++)
                        {
                            str += "3";
                        }
                        sc.WriteSector(3, 0, 256, util.AsciiToByteArray(str));

                        str = "";
                        for (int i = 0; i < 256; i++)
                        {
                            str += "4";
                        }
                        sc.WriteSector(4, 0, 256, util.AsciiToByteArray(str));

                        str = "";
                        for (int i = 0; i < 256; i++)
                        {
                            str += "4";
                        }
                        sc.WriteSector(4, 0, 256, util.AsciiToByteArray(str));

                        str = "";
                        for (int i = 0; i < 256; i++)
                        {
                            str += "5";
                        }
                        sc.WriteSector(5, 0, 256, util.AsciiToByteArray(str));

                        str = "";
                        for (int i = 0; i < 256; i++)
                        {
                            str += "6";
                        }
                        sc.WriteSector(6, 0, 256, util.AsciiToByteArray(str));

                        str = "";
                        for (int i = 0; i < 128; i++)
                        {
                            str += "7";
                        }
                        sc.WriteSector(7, 0, 128, util.AsciiToByteArray(str));

                        str = "";
                        for (int i = 0; i < 128; i++)
                        {
                            str += "8";
                        }
                        sc.WriteSector(8, 0, 128, util.AsciiToByteArray(str));

                        str = "";
                        for (int i = 0; i < 128; i++)
                        {
                            str += "9";
                        }
                        sc.WriteSector(9, 0, 128, util.AsciiToByteArray(str));

                        str = "";
                        for (int i = 0; i < 128; i++)
                        {
                            str += "A";
                        }
                        sc.WriteSector(10, 0, 128, util.AsciiToByteArray(str));

                        str = "";
                        for (int i = 0; i < 128; i++)
                        {
                            str += "B";
                        }
                        sc.WriteSector(11, 0, 128, util.AsciiToByteArray(str));

                        str = "";
                        for (int i = 0; i < 128; i++)
                        {
                            str += "C";
                        }
                        sc.WriteSector(12, 0, 128, util.AsciiToByteArray(str));

                        str = "";
                        for (int i = 0; i < 128; i++)
                        {
                            str += "D";
                        }
                        sc.WriteSector(13, 0, 128, util.AsciiToByteArray(str));

                        str = "";
                        for (int i = 0; i < 128; i++)
                        {
                            str += "E";
                        }
                        sc.WriteSector(14, 0, 128, util.AsciiToByteArray(str));

                        str = "";
                        for (int i = 0; i < 128; i++)
                        {
                            str += "F";
                        }
                        sc.WriteSector(15, 0, 128, util.AsciiToByteArray(str));

                        str = "";
                        for (int i = 0; i < 128; i++)
                        {
                            str += "a";
                        }
                        sc.WriteSector(16, 0, 128, util.AsciiToByteArray(str));

                        str = "";
                        for (int i = 0; i < 64; i++)
                        {
                            str += "b";
                        }
                        sc.WriteSector(17, 0, 64, util.AsciiToByteArray(str));

                        str = "";
                        for (int i = 0; i < 64; i++)
                        {
                            str += "c";
                        }
                        sc.WriteSector(18, 0, 64, util.AsciiToByteArray(str));

                        str = "";
                        for (int i = 0; i < 64; i++)
                        {
                            str += "d";
                        }
                        sc.WriteSector(19, 0, 64, util.AsciiToByteArray(str));

                        str = "";
                        for (int i = 0; i < 64; i++)
                        {
                            str += "e";
                        }
                        sc.WriteSector(20, 0, 64, util.AsciiToByteArray(str));

                        str = "";
                        for (int i = 0; i < 64; i++)
                        {
                            str += "f";
                        }
                        sc.WriteSector(21, 0, 64, util.AsciiToByteArray(str));

                        str = "";
                        for (int i = 0; i < 64; i++)
                        {
                            str += "g";
                        }
                        sc.WriteSector(18, 0, 64, util.AsciiToByteArray(str));

                        str = "";
                        for (int i = 0; i < 64; i++)
                        {
                            str += "h";
                        }
                        sc.WriteSector(19, 0, 64, util.AsciiToByteArray(str));

                        str = "";
                        for (int i = 0; i < 64; i++)
                        {
                            str += "i";
                        }
                        sc.WriteSector(20, 0, 64, util.AsciiToByteArray(str));

                        str = "";
                        for (int i = 0; i < 64; i++)
                        {
                            str += "j";
                        }
                        sc.WriteSector(21, 0, 64, util.AsciiToByteArray(str));

                        str = "";
                        for (int i = 0; i < 64; i++)
                        {
                            str += "k";
                        }
                        sc.WriteSector(22, 0, 64, util.AsciiToByteArray(str));

                        str = "";
                        for (int i = 0; i < 64; i++)
                        {
                            str += "l";
                        }
                        sc.WriteSector(23, 0, 64, util.AsciiToByteArray(str));

                        str = "";
                        for (int i = 0; i < 64; i++)
                        {
                            str += "m";
                        }
                        sc.WriteSector(24, 0, 64, util.AsciiToByteArray(str));

                        str = "";
                        for (int i = 0; i < 64; i++)
                        {
                            str += "n";
                        }
                        sc.WriteSector(25, 0, 64, util.AsciiToByteArray(str));

                        str = "";
                        for (int i = 0; i < 64; i++)
                        {
                            str += "o";
                        }
                        sc.WriteSector(26, 0, 64, util.AsciiToByteArray(str));

                        str = "";
                        for (int i = 0; i < 64; i++)
                        {
                            str += "p";
                        }
                        sc.WriteSector(27, 0, 64, util.AsciiToByteArray(str));

                        str = "";
                        for (int i = 0; i < 64; i++)
                        {
                            str += "q";
                        }
                        sc.WriteSector(28, 0, 64, util.AsciiToByteArray(str));

                        str = "";
                        for (int i = 0; i < 64; i++)
                        {
                            str += "r";
                        }
                        sc.WriteSector(29, 0, 64, util.AsciiToByteArray(str));

                        str = "";
                        for (int i = 0; i < 64; i++)
                        {
                            str += "s";
                        }
                        sc.WriteSector(30, 0, 64, util.AsciiToByteArray(str));

                        str = "";
                        for (int i = 0; i < 64; i++)
                        {
                            str += "t";
                        }
                        sc.WriteSector(31, 0, 64, util.AsciiToByteArray(str));

                        str = "";
                        for (int i = 0; i < 64; i++)
                        {
                            str += "u";
                        }
                        sc.WriteSector(32, 0, 64, util.AsciiToByteArray(str));

                        str = "";
                        for (int i = 0; i < 64; i++)
                        {
                            str += "v";
                        }
                        sc.WriteSector(33, 0, 64, util.AsciiToByteArray(str));

                        str = "";
                        for (int i = 0; i < 64; i++)
                        {
                            str += "w";
                        }
                        sc.WriteSector(34, 0, 64, util.AsciiToByteArray(str));

                        str = "";
                        for (int i = 0; i < 64; i++)
                        {
                            str += "x";
                        }
                        sc.WriteSector(35, 0, 64, util.AsciiToByteArray(str));

                        str = "";
                        for (int i = 0; i < 64; i++)
                        {
                            str += "y";
                        }
                        sc.WriteSector(36, 0, 64, util.AsciiToByteArray(str));
                    }
                    MessageBox.Show("Finish, span " + DateTime.Now.Subtract(start).TotalSeconds.ToString());
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            //for activation
            sc.SelectApplet(this.cbReader.SelectedIndex, this.cbSAM.SelectedIndex);
            sc.AuthenticateSL1();
            sc.getUmidFile(txtPrimaryLeft.Text, UMIDLibrary.AllCardTech_Smart_Card.UMID_Fields.BIOMETRIC_LEFT_PRIMARY_FINGER);
            //finger print matching here
            sc.AuthenticateSL2(util.AsciiToByteArray(this.txtUserPin.Text));
            sc.UMIDCard_Activate(util.AsciiToByteArray(this.txtUserPin.Text), 6).ToString();
        }

        private void btnRewrite_Click(object sender, EventArgs e)
        {
            this.lstBox.Items.Clear();
            if (sc.SelectApplet(this.cbReader.SelectedIndex, this.cbSAM.SelectedIndex))
            {

                this.lstBox.Items.Add("CRN=" + System.Text.ASCIIEncoding.ASCII.GetString(this.sc.get_getUmidData(UMIDLibrary.AllCardTech_Smart_Card.UMID_Fields.CRN)));

                if (!sc.AuthenticateSL1())
                {
                    MessageBox.Show("SL1 Failed");
                    return;
                }

                this.lstBox.Items.Add("Firstname = " + System.Text.ASCIIEncoding.ASCII.GetString(this.sc.get_getUmidData(UMIDLibrary.AllCardTech_Smart_Card.UMID_Fields.FIRST_NAME)).Trim());

                //Rewriting
                if (this.sc.CheckVersion().Equals(UMIDLibrary.AllCardTech_Smart_Card.UMID0))
                {
                    //old umid                                    
                    //get keys from the sam

                    sc.DisconnectApplet();

                    //for security retrieving the keys from SAM is not included in the DLL , 
                    //this should be done on the application which will retrieve the keys

                    //initialize reader                    
                    String message = "";
                    AllCardTech_PCSC sam = new AllCardTech_PCSC();
                    sam.InitializeReaders(new ComboBox(), ref message);
                    AllCardTech_Util util = new AllCardTech_Util();

                    if (!sam.ConnectCard(this.cbSAM.SelectedIndex, ref message))
                    {
                        MessageBox.Show("Failed to connect to card");
                        return;
                    }

                    //1. authenticate first by answering the random number generated                                                            
                    sam.SendAPDU("00a404000d47534953524557524954455231");
                    if (sam.LastByteReceived.Contains("90 00"))
                    {
                        sam.SendAPDU("000a0000");
                        if (sam.LastByteReceived.Contains("90 00"))
                        {

                            byte[] generated = util.HexToBytes(sam.LastByteReceived.Substring(0, sam.LastByteReceived.Length - 6).Replace(" ", ""));
                            int val = BitConverter.ToInt16(generated, 0);
                            val = (val * 2 + 5);

                            sam.SendAPDU("00aa000002" + util.ConvertIntToHexStr(val, false, false));

                            if (sam.LastByteReceived.Contains("90 00"))
                            {
                                //0 = AMK
                                //1= CAK
                                //2 = IAK
                                //3 = 201 Read
                                //4 = 201 Write
                                //5 = 202 Read
                                //6 = 202 Write
                                //..
                                //73 = 236 Read
                                //74 = 236 Write

                                int keyNos = 75;
                                for (int i = 0; i < keyNos; i++)
                                {
                                    sam.SendAPDU("00c2" + i.ToString("X2") + "00");
                                    String key = sam.LastByteReceived.Substring(0, sam.LastByteReceived.Length - 6).Replace(" ", "");
                                    this.lstBox.Items.Add("Key " + i + " = " + key);
                                }

                                //reperso/rewrite old umid here

                                return;
                            }
                        }
                    }
                    MessageBox.Show("Failed to get keys");
                }

                else if (this.sc.CheckVersion().Equals(UMIDLibrary.AllCardTech_Smart_Card.UMID1))
                {
                    //read last rewriting log on the card

                    String initials = "", terminalId = "";
                    DateTime logDate = new DateTime();

                    if (sc.ReadLatestUMIDRewritingLog(ref initials, ref logDate, ref terminalId))
                    {
                        this.lstBox.Items.Add("Rewriting Log Initials = " + initials);
                        this.lstBox.Items.Add("Rewriting Log  TerminalId = " + terminalId);
                        this.lstBox.Items.Add("Rewriting Log Date = " + logDate);
                    }

                    //new umid
                    //prerequisite SL1                                                            

                    bool res = sc.RewriteFingerprintRightSecondary("ACC", "10001", System.IO.File.ReadAllBytes("190892159975_Lprimary_leftindex.ansi-fmr"));
                    Console.Write("TEST");

                    //if (sc.RewriteFingerprintLeftPrimary("ACC", "10001", System.IO.File.ReadAllBytes("190892159975_Lprimary_leftindex.ansi-fmr")) &&
                    //    sc.RewriteFingerprintLeftSecondary("ACC", "10001", System.IO.File.ReadAllBytes("190892159975_Lbackup_leftthumb.ansi-fmr")) &&
                    //    sc.RewriteFingerprintRightPrimary("ACC", "10001", System.IO.File.ReadAllBytes("190892159975_Rprimary_rightindex.ansi-fmr")) &&
                    //    sc.RewriteFingerprintRightSecondary("ACC", "10001", System.IO.File.ReadAllBytes("190892159975_Rbackup_rightthumb.ansi-fmr")))
                    //{
                    //    MessageBox.Show("Rewriting Success");
                    //}
                    //else
                    //{
                    //    MessageBox.Show("Rewriting Failed");
                    //}
                }
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {

        }
    }
}
