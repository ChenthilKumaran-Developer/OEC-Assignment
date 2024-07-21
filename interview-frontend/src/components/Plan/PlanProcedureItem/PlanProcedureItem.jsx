import React, { useState, useEffect } from "react";
import ReactSelect from "react-select";
import { UserAssign, getUserAssign, deleteUserAssign } from "../../../api/api";

const PlanProcedureItem = ({ planId, procedure, users }) => {
    const [selectedUsers, setSelectedUsers] = useState([]);

    useEffect(() => {
        const attachUsersAssign = async () => {
            try {
                const _response = await getUserAssign(procedure.procedureId);
                const _usersAssign = _response.map(user => ({
                    value: user.userId,
                    label: user.userName,
                    userProcedureId: user.userProcedureId,
                }));
                setSelectedUsers(_usersAssign);
            } catch (error) {
                console.error("Something Went Wrong Assigned User:", error);
            }
        };
        attachUsersAssign();
    }, [procedure.procedureId]);

    const handleAssignUserToProcedure = async (e) => {
        const newUsers = e.map(option => ({
            userId: option.value,
            userName: option.label,
        }));

        // Determine added and removed users
        const addedUsers = newUsers.filter(newUser => !selectedUsers.some(user => user.value === newUser.userId));
        const removedUsers = selectedUsers.filter(user => !newUsers.some(newUser => newUser.userId === user.value));

        // Update the state
        setSelectedUsers(e);

        // Handle added users
        if (addedUsers.length > 0) {
            try {
                await UserAssign(planId, procedure.procedureId, addedUsers);
            } catch (error) {
                console.error("Something Went Wrong Assigning User:", error);
            }
        }

        // Handle removed users
        if (removedUsers.length > 0) {
            try {
                await Promise.all(removedUsers.map(user => deleteUserAssign(user.userProcedureId)));
            } catch (error) {
                console.error("Something Went Wrong Deleting User:", error);
            }
        }
    };

    return (
        <div className="py-2">
            <div>{procedure.procedureTitle}</div>
            <ReactSelect
                className="mt-2"
                placeholder="Select User to Assign"
                isMulti={true}
                options={users}
                value={selectedUsers}
                onChange={(e) => handleAssignUserToProcedure(e)}
            />
        </div>
    );
};

export default PlanProcedureItem;