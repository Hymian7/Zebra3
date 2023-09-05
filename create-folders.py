import os
import random
import argparse
from reportlab.lib.pagesizes import letter
from reportlab.pdfgen import canvas

# Liste von Taylor Swift-Songtiteln
song_titles = ["Love Story", "You Belong with Me", "Shake It Off", "Blank Space", "Bad Blood",
               "Delicate", "Wildest Dreams", "All Too Well", "Red", "Style",
               "I Knew You Were Trouble", "Lover", "Fearless", "22", "Mine",
               "Our Song", "Teardrops on My Guitar", "Mean", "Speak Now", "Enchanted",
               "Begin Again", "The Man", "Cardigan", "Exile", "The 1",
               "Getaway Car", "The Last Great American Dynasty", "Long Live", "White Horse", "Dear John"]

# Liste von Teilen
parts = ["TRP1", "TRP2", "KLRNT1", "KLRNT2", "FLT",
         "POS1", "POS2", "TUBA", "SCHLGZG"]

def create_pdf(file_path, piece, part):
    c = canvas.Canvas(file_path, pagesize=letter)
    c.drawString(100, 750, f"%PIECE%: {piece}")
    c.drawString(100, 725, f"%PART%: {part}")
    c.save()

def create_folders_and_pdfs(root_directory):
    # Erstelle das Root-Verzeichnis, wenn es nicht existiert
    if not os.path.exists(root_directory):
        os.makedirs(root_directory)

    # Erstelle 30 Ordner mit zufälligen Taylor Swift-Songtiteln
    for i in range(1, 31):
        folder_name = random.choice(song_titles)
        folder_path = os.path.join(root_directory, folder_name)

        # Überprüfe, ob das Verzeichnis bereits existiert, bevor du es erstellst
        if not os.path.exists(folder_path):
            os.makedirs(folder_path)

            # Erstelle PDF-Dateien in jedem Ordner
            for part in parts:
                pdf_name = f"{folder_name}_{part}.pdf"
                pdf_path = os.path.join(folder_path, pdf_name)
                create_pdf(pdf_path, folder_name, part)

if __name__ == "__main__":
    parser = argparse.ArgumentParser(description="Erstelle Ordner und PDFs")
    parser.add_argument("root_directory", type=str, help="Pfad zum Root-Verzeichnis")
    args = parser.parse_args()
    
    create_folders_and_pdfs(args.root_directory)
