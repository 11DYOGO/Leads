export default function InvitedCard({ lead, onAccept, onDecline }) {
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
        Contact: <b>{lead.contactFirstName}</b>
      </p>

      <p className="desc">{lead.description}</p>

      <div className="row space">
        <b className="price">R$ {Number(lead.price).toFixed(2)}</b>
        <div className="row gap">
          <button onClick={() => onAccept(lead.id)}>
            <i className="fa-solid fa-check"></i> Accept
          </button>
          <button className="secondary" onClick={() => onDecline(lead.id)}>
            <i className="fa-solid fa-xmark"></i> Decline
          </button>
        </div>
      </div>
    </div>
  );
}
