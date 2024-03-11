keytool -export -rfc -keystore ChronoWiz_Keystore.jks -alias upload -file ChronoWiz_upload_certificate.pem

keytool -export -rfc -keystore upload-keystore.jks -alias upload -file upload_certificate.pem