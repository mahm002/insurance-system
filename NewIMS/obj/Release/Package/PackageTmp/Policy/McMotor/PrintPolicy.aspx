<%@ Page Language="VB" AutoEventWireup="true" 
    CodeBehind="PrintPolicy.aspx.vb" 
    Inherits="PrintPolicy" %>

<!DOCTYPE html>
<html>
<head runat="server">
    <title>Print Policy Document</title>
    <style>
        body {
            margin: 0;
            padding: 0;
            font-family: Arial, sans-serif;
            background: #f5f5f5;
        }
        
        .loading {
            position: fixed;
            top: 50%;
            left: 50%;
            transform: translate(-50%, -50%);
            text-align: center;
            background: white;
            padding: 30px;
            border-radius: 10px;
            box-shadow: 0 2px 10px rgba(0,0,0,0.1);
        }
        
        .spinner {
            border: 5px solid #f3f3f3;
            border-top: 5px solid #3498db;
            border-radius: 50%;
            width: 50px;
            height: 50px;
            animation: spin 1s linear infinite;
            margin: 0 auto 20px;
        }
        
        @keyframes spin {
            0% { transform: rotate(0deg); }
            100% { transform: rotate(360deg); }
        }
        
        #pdfFrame {
            width: 100%;
            height: 100vh;
            border: none;
        }
        
        .hidden {
            display: none;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <!-- Hidden fields to store data from code-behind -->
        <asp:HiddenField ID="pdfUrlHidden" runat="server" />
        <asp:HiddenField ID="policyNumberHidden" runat="server" />
        
        <!-- Loading screen -->
        <div id="loading" class="loading">
            <div class="spinner"></div>
            <h3>تحميل الوثيقة للطباعة...</h3>
            <p>يرجى الانتظار ريثما نقوم بتجهيز الوثيقة للطباعة </p>
        </div>
        
        <!-- PDF will be loaded here -->
        <iframe id="pdfFrame" class="hidden"></iframe>
    </form>
    
    <script>
        // Get data from hidden fields
        var pdfUrl = document.getElementById('<%= pdfUrlHidden.ClientID %>').value;
        var policyNumber = document.getElementById('<%= policyNumberHidden.ClientID %>').value;

        // Auto-start when page loads
        window.onload = function () {
            if (pdfUrl && policyNumber) {
                startAutoPrintProcess();
            } else {
                document.getElementById('loading').innerHTML =
                    '<div style="color: red;">Error: Could not load document. Missing policy number.</div>';
            }
        };

        function startAutoPrintProcess() {
            console.log('Starting auto-print process for policy:', policyNumber);

            var pdfFrame = document.getElementById('pdfFrame');
            var loadingDiv = document.getElementById('loading');

            // Set the PDF URL
            pdfFrame.src = pdfUrl;

            // When PDF loads, trigger print
            pdfFrame.onload = function () {
                console.log('PDF loaded successfully');

                // Hide loading, show PDF
                loadingDiv.classList.add('hidden');
                pdfFrame.classList.remove('hidden');

                // Wait 1 second to ensure PDF is fully rendered, then print
                setTimeout(function () {
                    triggerPrint();
                }, 1000);
            };

            // Handle errors
            pdfFrame.onerror = function () {
                console.error('Failed to load PDF');
                loadingDiv.innerHTML =
                    '<div style="color: red; padding: 20px;">' +
                    '<h3>Error Loading Document</h3>' +
                    '<p>Could not load the policy document. Please try again.</p>' +
                    '<button onclick="window.location.reload()" style="padding: 10px 20px; background: #3498db; color: white; border: none; border-radius: 5px; cursor: pointer;">Retry</button>' +
                    '</div>';
            };

            // Set timeout in case loading takes too long
            setTimeout(function () {
                if (!pdfFrame.classList.contains('hidden')) {
                    loadingDiv.innerHTML =
                        '<div style="color: orange; padding: 20px;">' +
                        '<h3>Taking longer than expected...</h3>' +
                        '<p>The document is still loading. You can try:</p>' +
                        '<button onclick="window.location.reload()" style="padding: 10px 20px; background: #3498db; color: white; border: none; border-radius: 5px; cursor: pointer; margin: 5px;">Retry Loading</button>' +
                        '<button onclick="openDirectPDF()" style="padding: 10px 20px; background: #2ecc71; color: white; border: none; border-radius: 5px; cursor: pointer; margin: 5px;">Open PDF Directly</button>' +
                        '</div>';
                }
            }, 10000); // 10 second timeout
        }

        function triggerPrint() {
            console.log('Attempting to trigger print dialog...');

            var pdfFrame = document.getElementById('pdfFrame');

            try {
                // Method 1: Try to print the iframe content
                if (pdfFrame.contentWindow && pdfFrame.contentWindow.print) {
                    pdfFrame.contentWindow.focus(); // Focus the iframe
                    pdfFrame.contentWindow.print();
                    console.log('Print dialog triggered via iframe');
                    return true;
                }
            } catch (e) {
                console.warn('Iframe print failed:', e.message);
            }

            try {
                // Method 2: Fallback to window.print()
                window.focus();
                window.print();
                console.log('Print dialog triggered via window.print()');
                return true;
            } catch (e) {
                console.error('Window print failed:', e.message);

                // Method 3: Show manual instructions
                showManualPrintInstructions();
                return false;
            }
        }

        function showManualPrintInstructions() {
            var loadingDiv = document.getElementById('loading');
            loadingDiv.classList.remove('hidden');
            loadingDiv.innerHTML =
                '<div style="text-align: center; padding: 20px;">' +
                '<h3>Print Instructions</h3>' +
                '<p>The document has loaded but automatic printing failed.</p>' +
                '<p>Please press <strong>Ctrl + P</strong> on your keyboard to print,</p>' +
                '<p>or use the print option in your browser\'s menu.</p>' +
                '<button onclick="triggerPrint()" style="padding: 10px 20px; background: #3498db; color: white; border: none; border-radius: 5px; cursor: pointer; margin: 10px;">Try Auto-Print Again</button>' +
                '<button onclick="window.close()" style="padding: 10px 20px; background: #95a5a6; color: white; border: none; border-radius: 5px; cursor: pointer; margin: 10px;">Close Window</button>' +
                '</div>';
        }

        function openDirectPDF() {
            window.open(pdfUrl, '_blank');
        }

        // Listen for afterprint event (when print dialog closes)
        window.addEventListener('afterprint', function (event) {
            console.log('Print dialog closed');

            // Optional: Ask user if they want to close the window
            setTimeout(function () {
                if (confirm('Print dialog closed. Would you like to close this window?')) {
                    window.close();
                }
            }, 500);
        });
    </script>
</body>
</html>