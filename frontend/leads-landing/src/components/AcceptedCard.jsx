export default function AcceptedCard({ lead }) {
  const fullName = `${lead.contactFirstName ?? ''} ${
    lead.contactLastName ?? ''
  }`.trim();

  const dataIso = lead.dateCreated.endsWith('Z')
    ? lead.dateCreated
    : lead.dateCreated + 'Z';
  const dataLocal = new Date(dataIso).toLocaleString('pt-BR', {
    timeZone: 'America/Sao_Paulo',
    hour12: false,
  });

  return (
    <div className="card">
      <div className="row space">
        <div className="row gap">
          <strong>{lead.id}°</strong>
          {lead.category && <span className="pill">{lead.category}</span>}
          {lead.suburb && <span className="muted">{lead.suburb}</span>}
        </div>
        <span className="muted">{dataLocal}</span>
      </div>

      <p className="muted" style={{ margin: '6px 0' }}>
        Contact: <b>{fullName}</b>
      </p>

      <div
        className="row gap"
        style={{ flexWrap: 'wrap', margin: '6px 0 10px' }}
      >
        {lead.contactPhone && (
          <span className="chip">
            <i className="fa-solid fa-phone"></i> {lead.contactPhone}
          </span>
        )}
        {lead.contactEmail && (
          <span className="chip">
            <i className="fa-solid fa-envelope"></i> {lead.contactEmail}
          </span>
        )}
      </div>

      <p className="desc">{lead.description}</p>

      <div className="row space">
        <b className="price">R$ {Number(lead.price).toFixed(2)}</b>
      </div>
    </div>
  );
}
