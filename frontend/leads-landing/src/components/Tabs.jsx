export default function Tabs({ active, onChange }) {
  return (
    <div className="tabs">
      <button
        className={active === 'invited' ? 'tab active' : 'tab'}
        onClick={() => onChange('invited')}
        title="Leads com status NEW">
        <i className="fa-solid fa-user-plus"></i> Invited
      </button>
      <button
        className={active === 'accepted' ? 'tab active' : 'tab'}
        onClick={() => onChange('accepted')}
        title="Leads com status ACCEPTED">
        <i className="fa-solid fa-circle-check"></i> Accepted
      </button>
    </div>
  );
}
