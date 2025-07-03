async function enviarTexto() {
    const texto = document.getElementById("userInput").value;

    const resposta = await fetch('/Form/ProcessText', {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json'
        },
        body: JSON.stringify({ Text: texto }) // com "T" maiúsculo
git
    });

    if (!resposta.ok) {
        const erro = await resposta.text();
        alert("Erro ao processar o texto. Detalhes: " + erro);
        return;
    }

    const data = await resposta.json();

    document.getElementById("data").value = data.data || "";
    document.getElementById("horaI").value = data.horaI || "";
    document.getElementById("horaFim").value = data.horaFim || "";
    document.getElementById("ausente").value = data.ausente || "";
    document.getElementById("reg").value = data.reg || "";
}