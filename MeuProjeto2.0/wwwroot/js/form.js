async function enviarTexto() {
    const texto = document.getElementById("userInput").value;

    const resposta = await fetch('/Form/ProcessText', {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json'
        },
        body: JSON.stringify({ Text: texto }) // com "T" maiúsculo
    });

    if (!resposta.ok) {
        const erro = await resposta.text();
        alert("Erro ao processar o texto. Detalhes: " + erro);
        return;
    }

    const data = await resposta.json();

    document.getElementById("data").value = data.data || new Date().toISOString().split('T')[0];
    document.getElementById("horaFim").value = data.horaFim || new Date().toLocaleTimeString([], { hour: '2-digit', minute: '2-digit' });

    document.getElementById("horaI").value = data.horaI || "";
    document.getElementById("ausente").value = data.ausente || "";
    document.getElementById("reg").value = data.reg || "";

    document.getElementById("empresa").value = data.empresa || "";
    document.getElementById("empresaDescricao").value = data.empresaDescricao || "";
    document.getElementById("pedidoPor").value = data.pedidoPor || "";
    document.getElementById("pedido").value = data.pedido || "";
    document.getElementById("gastos").value = data.gastos || "";
    document.getElementById("tempoDeslocacao").value = data.tempoDeslocacao || "";
}

async function guardarFormulario() {
    const formData = {
        data: document.getElementById("data").value,
        horaI: document.getElementById("horaI").value,
        horaFim: document.getElementById("horaFim").value,
        ausente: document.getElementById("ausente").value,
        reg: document.getElementById("reg").value,
        empresa: document.getElementById("empresa").value,
        empresaDescricao: document.getElementById("empresaDescricao").value,
        pedidoPor: document.getElementById("pedidoPor").value,
        pedido: document.getElementById("pedido").value,
        gastos: document.getElementById("gastos").value,
        tempoDeslocacao: document.getElementById("tempoDeslocacao").value
    };

    try {
        const response = await fetch('/Form/Guardar', {
            method: 'POST',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify(formData)
        });

        if (response.ok) {
            alert("Formulário guardado com sucesso!");
        } else {
            const errorText = await response.text();
            alert("Erro ao guardar: " + errorText);
        }
    } catch (error) {
        alert("Erro ao guardar: " + error.message);
    }
}